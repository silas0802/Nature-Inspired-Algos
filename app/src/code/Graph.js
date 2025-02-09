// filepath: /c:/Users/silas/Documents/Hobby Projects/Mackrill/app/src/js/Graph.js
import React, { useRef, useEffect } from 'react';
import * as d3 from 'd3';

const Graph = ({ graphs, xName, yName, labels, noPoints, sorted}) => {
  const svgRef = useRef();
  useEffect(() => {
    const svg = d3.select(svgRef.current)
      .attr('width', 500)
      .attr('height', 500)
      
    // Clear previous elements
    svg.selectAll('*').remove();

    const lines = [];
    const allPoints = graphs.length>1 ? graphs.flat() : graphs[0];
    const maxX = Math.max(...allPoints.map(p => p.x));
    const maxY = Math.max(...allPoints.map(p => p.y));
    const colors = ['red', 'blue', 'green', 'yellow', 'purple', 'orange', 'pink', 'brown', 'cyan', 'magenta'];
    
    for (let j = 0; j < graphs.length; j++) {
      const points = graphs[j];
      if (sorted){
        points.sort((a, b) => a.x - b.x);
      }
      lines.push([]);
      for (let i = 0; i < points.length - 1; i++) {
        lines[j].push({ x1: points[i].x, y1: points[i].y, x2: points[i + 1].x, y2: points[i + 1].y });
      }
    }

    
    // Add axes
    const xScale = d3.scaleLinear().domain([0, maxX]).range([0, 450]);
    const yScale = d3.scaleLinear().domain([0, maxY]).range([450, 0]);

    const xAxis = d3.axisBottom(xScale).ticks(10).tickFormat(d3.format(".0f"));
    const yAxis = d3.axisLeft(yScale).ticks(10).tickFormat(d3.format(".0f"));

    

    // Add lines
    for (let i = 0; i < lines.length; i++) {
      svg.selectAll(`.line-${i}`)
        .data(lines[i])
        .enter()
        .append('line')
        .attr('class', `line line-${i}`) // Add class
        .attr('x1', d => d.x1 * 450 / maxX + 40)
        .attr('y1', d => 475 - d.y1 * 450 / maxY)
        .attr('x2', d => d.x2 * 450 / maxX + 40)
        .attr('y2', d => 475 - d.y2 * 450 / maxY)
        .attr('stroke', colors[i % colors.length]);

      // Add line labels
      svg.append('text')
        .attr('x', 490)
        .attr('y', 30 + i * 20)
        .attr('text-anchor', 'end')
        .attr('font-size', '12px')
        .attr('fill', colors[i % colors.length])
        .text(labels[i]);
    }
    
    // Add points
    if (!noPoints){
      svg.selectAll('circle')
      .data(allPoints)
      .enter()
      .append('circle')
      .attr('class', 'point') // Add class
      .attr('cx', d => d.x*450/maxX + 40)
      .attr('cy', d => 475 - d.y*450/maxY)
    }


    svg.append('g')
      .attr('transform', 'translate(40, 475)')
      .call(xAxis);

    svg.append('g')
      .attr('transform', 'translate(40, 25)')
      .call(yAxis);

    // Add axis labels
    svg.append('text')
      .attr('x', 250)
      .attr('y', 498)
      .attr('text-anchor', 'middle')
      .attr('font-size', '12px')
      .attr('fill', 'white')
      .text(xName);

    svg.append('text')
      .attr('x', -225)
      .attr('y', 10)
      .attr('text-anchor', 'middle')
      .attr('font-size', '12px')
      .attr('fill', 'white')
      .attr('transform', 'rotate(-90)')
      .text(yName);
  }, [graphs, noPoints, sorted, xName, yName, labels]);

  return <svg ref={svgRef} className="Graph"></svg>; // Apply class
};

export default Graph;