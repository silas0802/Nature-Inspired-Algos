import React, { useRef, useEffect } from 'react';
import * as d3 from 'd3';

const BitDiagram = ({ bitEntries }) => {
  const svgRef = useRef();

  const centerX = 150;
  const centerY = 250;
  const rx = 150;
  const ry = 250;

  const getEllipseWidthAtY = (y) => {
    
    // Calculate the width at y using the ellipse equation
    const term = 1 - Math.pow(y - centerY, 2) / Math.pow(ry, 2);
    if (term < 0) {
      // y is outside the ellipse
      return 0;
    }
    
    return 2 * rx * Math.sqrt(term);
  };

  useEffect(() => {
    const svg = d3.select(svgRef.current)
      .attr('width', 300)
      .attr('height', 500);

    // Clear previous elements
    svg.selectAll('*').remove();

    const colors = ['red', 'blue', 'green', 'yellow', 'purple', 'orange', 'pink', 'brown', 'cyan', 'magenta'];
    
    // Define points array
    const points = [];
    for (let i = 0; i < bitEntries.length; i++) { // For each algorithm
      for (let j = 0; j < bitEntries[i].length; j++) { // For each iteration
        const bits = bitEntries[i][j];
        const length = bits.length;
        const oneCount = bits.reduce((acc, bit) => acc + bit, 0);
        const vPerc = oneCount / bits.length;
        const yVal = (1 - vPerc) * 500;
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
      .attr('fill', '#3d466b') // Background color
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

  }, [bitEntries]);

  return <svg ref={svgRef} className="BitDiagram"></svg>; // Apply class
};

export default BitDiagram;