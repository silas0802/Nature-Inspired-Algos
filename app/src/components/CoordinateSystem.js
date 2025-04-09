import React, { useRef, useEffect, useState } from 'react';
import * as d3 from 'd3';
import '../style/TSPPage.css';

const CoordinateSystem = ({ points, labels }) => {
  const svgRef = useRef();
  const containerRef = useRef();
  const [dimensions, setDimensions] = useState({ width: 500, height: 500 });
  
  // Update dimensions when container size changes
  useEffect(() => {
    const updateDimensions = () => {
      if (containerRef.current) {
        const { width, height } = containerRef.current.getBoundingClientRect();
        setDimensions({ 
          width: Math.floor(width), 
          height: Math.floor(height) 
        });
      }
    };

    updateDimensions();

    const resizeObserver = new ResizeObserver(updateDimensions);
    const currentContainerRef = containerRef.current;

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
    const colors = ['red', 'blue', 'green', 'yellow', 'purple', 'orange', 'pink', 'brown', 'cyan', 'magenta'];

    if (dimensions.width === 0) return;

    const svg = d3.select(svgRef.current)
      .attr('width', dimensions.width)
      .attr('height', dimensions.height);

    // Clear previous elements
    svg.selectAll('*').remove();

    // Calculate lines
    const lines = [];
    for (let algoI = 0; algoI < points.length; algoI++) {
      const nodes = points[algoI];
      lines.push({ x1: nodes[0].x, y1: nodes[0].y, x2: nodes[nodes.length - 1].x, y2: nodes[nodes.length - 1].y, algoI: algoI });
      for (let i = 0; i < nodes.length - 1; i++) {
        lines.push({ x1: nodes[i].x, y1: nodes[i].y, x2: nodes[i + 1].x, y2: nodes[i + 1].y , algoI: algoI});
      }
      
    }
    

    // Add lines
    svg.selectAll('line')
      .data(lines)
      .enter()
      .append('line')
      .attr('class', 'line') // Add class
      .attr('x1', d => d.x1)
      .attr('y1', d => d.y1)
      .attr('x2', d => d.x2)
      .attr('y2', d => d.y2)
      .attr('stroke', d => colors[d.algoI % colors.length]);

    // Add points
    // svg.selectAll('circle')
    //   .data(points)
    //   .enter()
    //   .append('circle')
    //   .attr('class', 'point') // Add class
    //   .attr('cx', d => d.x)
    //   .attr('cy', d => d.y);

  }, [points, dimensions]);

  return (
    <div ref={containerRef} className="CoordinateSystem" style={{ width: '100%', height: '100%' }}>
      <svg ref={svgRef}></svg>
    </div>
  );
};

export default CoordinateSystem;