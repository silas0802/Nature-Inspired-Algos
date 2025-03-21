import React, { useRef, useEffect, useState } from 'react';
import * as d3 from 'd3';

const BitDiagram = ({ bitEntries }) => {
  const svgRef = useRef();
  const containerRef = useRef();
  const [dimensions, setDimensions] = useState({ width: 300, height: 500 });

  // Update dimensions when container size changes
  useEffect(() => {
    const updateDimensions = () => {
      if (containerRef.current) {
        const { width } = containerRef.current.getBoundingClientRect();
        const height = Math.floor((width * 5) / 3); // Maintain 3:5 aspect ratio
        setDimensions({
          width: Math.floor(width), // Ensure width is an integer
          height // Dynamically calculated height
        });
      }
    };

    updateDimensions();

    // Set up resize observer
    const resizeObserver = new ResizeObserver(updateDimensions);
    const currentContainerRef = containerRef.current; // Store ref value

    if (currentContainerRef) {
      resizeObserver.observe(currentContainerRef);
    }

    window.addEventListener('resize', updateDimensions);

    return () => {
      if (currentContainerRef) {
        resizeObserver.unobserve(currentContainerRef);
      }
      window.removeEventListener('resize', updateDimensions);
    };
  }, []);

  useEffect(() => {
    const svg = d3.select(svgRef.current)
      .attr('width', dimensions.width)
      .attr('height', dimensions.height);

    // Clear previous elements
    svg.selectAll('*').remove();

    const centerX = dimensions.width / 2;
    const centerY = dimensions.height / 2;
    const rx = dimensions.width / 2;
    const ry = dimensions.height / 2;

    const getEllipseWidthAtY = (y) => {
      // Calculate the width at y using the ellipse equation
      const term = 1 - Math.pow(y - centerY, 2) / Math.pow(ry, 2);
      if (term < 0) {
        // y is outside the ellipse
        return 0;
      }
      
      return 2 * rx * Math.sqrt(term);
    };

    const colors = ['red', 'blue', 'green', 'yellow', 'purple', 'orange', 'pink', 'brown', 'cyan', 'magenta'];
    const points = [];
    for (let i = 0; i < bitEntries.length; i++) { // For each algorithm
      for (let j = 0; j < bitEntries[i].length; j++) { // For each iteration
        const bits = bitEntries[i][j];
        const length = bits.length;
        const oneCount = bits.reduce((acc, bit) => acc + bit, 0);
        const vPerc = oneCount / bits.length;
        const yVal = (1 - vPerc) * dimensions.height;
        const avrX = oneCount ? (bits.reduce((acc, bit, idx) => acc + bit * idx, 0) / oneCount) : 0;


        // Calculate theoretical min/max average positions for this oneCount and length
        const minPossibleAvg = (oneCount - 1) / 2;  // If all 1s are leftmost positions
        const maxPossibleAvg = length - 1 - (oneCount - 1) / 2;  // If all 1s are rightmost positions

        // Calculate ellipse boundaries at this y-coordinate
        const ellipseWidth = getEllipseWidthAtY(yVal);
        const xMin = centerX - ellipseWidth / 2;
        const xMax = centerX + ellipseWidth / 2;

        // Map avrX from its possible range to the ellipse width
        const xVal = oneCount > 0 
          ? (maxPossibleAvg === minPossibleAvg) 
            ? centerX  // If min and max are the same (e.g., only one '1'), place at center
            : xMin + ((avrX - minPossibleAvg) / (maxPossibleAvg - minPossibleAvg)) * (xMax - xMin)
          : centerX; // If no 1s, place at center

        points.push({ x: xVal, y: yVal, algorithmIndex: i });
      }
    }

    // Add an ellipse to create the elliptical background
    svg.append('ellipse')
      .attr('cx', centerX) // Center x
      .attr('cy', centerY) // Center y
      .attr('rx', rx) // Radius x
      .attr('ry', ry) // Radius y
      .attr('fill', '#3d466b')// Background color
      .attr('stroke', '#333') // Border color
      .attr('stroke-width', 2); // Border width

    // Add points with colors based on algorithm
    svg.selectAll('circle')
      .data(points)
      .enter()
      .append('circle')
      .attr('class', 'point') // Add class
      .attr('cx', d => d.x)
      .attr('cy', d => d.y)
      .attr('r', 5) // Set point radius
      .attr('fill', d => colors[d.algorithmIndex % colors.length]); // Apply color based on algorithm

  }, [bitEntries, dimensions]);

  return (
    <div ref={containerRef} className="BitDiagram" style={{ width: '100%' }}>
      <svg ref={svgRef}></svg>
    </div>
  );
};

export default BitDiagram;