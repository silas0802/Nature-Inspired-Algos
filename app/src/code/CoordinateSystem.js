import React, { useRef, useEffect } from 'react';
import * as d3 from 'd3';

const CoordinateSystem = ({ points }) => {
  const svgRef = useRef();

  useEffect(() => {
    const svg = d3.select(svgRef.current)
      .attr('width', 500)
      .attr('height', 500)


    // Clear previous elements
    svg.selectAll('*').remove();

    // Calculate lines
    const lines = [{ x1: points[0].x, y1: points[0].y, x2: points[points.length-1].x, y2: points[points.length-1].y }];
    for (let i = 0; i < points.length - 1; i++) {
      lines.push({ x1: points[i].x, y1: points[i].y, x2: points[i + 1].x, y2: points[i + 1].y });
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
      .attr('y2', d => d.y2);

    // Add points
    svg.selectAll('circle')
      .data(points)
      .enter()
      .append('circle')
      .attr('class', 'point') // Add class
      .attr('cx', d => d.x)
      .attr('cy', d => d.y)

  }, [points]);

  return <svg ref={svgRef}></svg>;
};

export default CoordinateSystem;