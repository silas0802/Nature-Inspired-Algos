import React, { useRef, useEffect } from 'react';
import * as d3 from 'd3';

const BitDiagram = ({ bitEntries }) => {
  const svgRef = useRef();

  useEffect(() => {
    const svg = d3.select(svgRef.current)
      .attr('width', 300)
      .attr('height', 500);

    // Clear previous elements
    svg.selectAll('*').remove();

    // Define points array
    const points = [];
    for (let i = 0; i < bitEntries.length; i++) {
      for (let j = 0; j < bitEntries[i].length; j++) {
        const bits = bitEntries[i][j]; // Correctly access bits
        
        const vPerc = bits.reduce((acc, bit) => acc + bit, 0) / bits.length;
        
        points.push({ x: 300/2, y: (1 - vPerc) * 500 });
      }
    }

    // Add an ellipse to create the elliptical background
    svg.append('ellipse')
      .attr('cx', 150) // Center x
      .attr('cy', 250) // Center y
      .attr('rx', 150) // Radius x
      .attr('ry', 250) // Radius y
      .attr('fill', '#3d466b') // Background color
      .attr('stroke', '#333') // Border color
      .attr('stroke-width', 2); // Border width

    // Add points
    svg.selectAll('circle')
      .data(points)
      .enter()
      .append('circle')
      .attr('class', 'point') // Add class
      .attr('cx', d => d.x)
      .attr('cy', d => d.y)
      .attr('r', 5); // Set point radius

  }, [bitEntries]);

  return <svg ref={svgRef} className="BitDiagram"></svg>; // Apply class
};

export default BitDiagram;