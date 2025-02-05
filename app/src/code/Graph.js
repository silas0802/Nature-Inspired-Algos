// filepath: /c:/Users/silas/Documents/Hobby Projects/Mackrill/app/src/js/Graph.js
import React, { useRef, useEffect } from 'react';
import * as d3 from 'd3';

const Graph = ({ graphs }) => {
  const svgRef = useRef();

  useEffect(() => {
    const svg = d3.select(svgRef.current)
      .attr('width', 500)
      .attr('height', 500)
      .style('border', '1px solid black')
      .style('background-color', '#3d466b'); // Apply background color

    // Clear previous elements
    svg.selectAll('*').remove();

    const lines = [];
    const allPoints = graphs.flat();
    const maxX = Math.max(...allPoints.map(p => p.x));
    const maxY = Math.max(...allPoints.map(p => p.y));
    for (let j = 0; j < graphs.length; j++) {
      const points = graphs[j];
      for (let i = 0; i < points.length - 1; i++) {
        lines.push({ x1: points[i].x * 450 / maxX, y1: points[i].y * 450 / maxY, x2: points[i + 1].x * 450 / maxX, y2: points[i + 1].y * 450 / maxY });
      }
    }
    for (let i = 0; i < allPoints.length; i++) {
      allPoints[i].x = allPoints[i].x * 450 / maxX;
      allPoints[i].y = allPoints[i].y * 450 / maxY;
    }

    // Add axes
    const xScale = d3.scaleLinear().domain([0, 450]).range([0, maxX]);
    const yScale = d3.scaleLinear().domain([0, 450]).range([maxY, 0]);

    const xAxis = d3.axisBottom(xScale).ticks(10);
    const yAxis = d3.axisLeft(yScale).ticks(10);

    

    // Add lines
    svg.selectAll('line')
      .data(lines)
      .enter()
      .append('line')
      .attr('class', 'line') // Add class
      .attr('x1', d => d.x1 + 25)
      .attr('y1', d => d.y1 + 25)
      .attr('x2', d => d.x2 + 25)
      .attr('y2', d => d.y2 + 25);

    // Add points
    svg.selectAll('circle')
      .data(allPoints)
      .enter()
      .append('circle')
      .attr('class', 'point') // Add class
      .attr('cx', d => d.x + 25)
      .attr('cy', d => d.y + 25)
      .attr('r', 5); // Ensure radius is set

    svg.append('g')
      .attr('transform', 'translate(40, 475)')
      .call(xAxis);

    svg.append('g')
      .attr('transform', 'translate(40, 25)')
      .call(yAxis);

    // Add axis labels
    svg.append('text')
      .attr('x', 250)
      .attr('y', 500)
      .attr('text-anchor', 'middle')
      .attr('font-size', '12px')
      .attr('fill', 'white')
      .text('X Axis');

    svg.append('text')
      .attr('x', -225)
      .attr('y', 10)
      .attr('text-anchor', 'middle')
      .attr('font-size', '12px')
      .attr('fill', 'white')
      .attr('transform', 'rotate(-90)')
      .text('Y Axis');
  }, [graphs]);

  return <svg ref={svgRef} className="Graph"></svg>; // Apply class
};

export default Graph;