import React, { useRef, useEffect, useState } from 'react';
import * as d3 from 'd3';

const Graph = ({ graphs, xName, yName, labels, noPoints, sorted}) => {
  const svgRef = useRef();
  const containerRef = useRef();
  const [dimensions, setDimensions] = useState({ width: 500, height: 500 });

  // Update dimensions when container size changes
  useEffect(() => {
    const updateDimensions = () => {
      if (containerRef.current) {
        const { width, height } = containerRef.current.getBoundingClientRect();
        setDimensions({ 
          width: Math.floor(width), // Ensure width is an integer
          height: Math.floor(height) // Ensure height is an integer
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
    if (dimensions.width === 0) return;

    const margin = { top: 40, right: 40, bottom: 50, left: 50 };
    const width = dimensions.width - margin.left - margin.right;
    const height = dimensions.height - margin.top - margin.bottom;
    
    const svg = d3.select(svgRef.current)
      .attr('width', dimensions.width)
      .attr('height', dimensions.height);
      
    // Clear previous elements
    svg.selectAll('*').remove();

    const g = svg.append('g')
      .attr('transform', `translate(${margin.left}, ${margin.top})`);

    const lines = [];
    const allPoints = graphs.length > 1 ? graphs.flat() : graphs[0];
    const maxX = Math.max(...allPoints.map(p => p.x));
    const maxY = Math.max(8, Math.max(...allPoints.map(p => p.y)));
    const colors = ['red', 'blue', 'green', 'yellow', 'purple', 'orange', 'pink', 'brown', 'cyan', 'magenta'];
    
    // Set up scales
    const xScale = d3.scaleLinear().domain([0, maxX]).range([0, width]);
    const yScale = d3.scaleLinear().domain([0, maxY]).range([height, 0]);

    for (let j = 0; j < graphs.length; j++) {
      const points = graphs[j];
      if (sorted) {
        points.sort((a, b) => a.x - b.x);
      }
      
      lines.push([]);
      for (let i = 0; i < points.length - 1; i++) {
        lines[j].push({ 
          x1: xScale(points[i].x), 
          y1: yScale(points[i].y), 
          x2: xScale(points[i + 1].x), 
          y2: yScale(points[i + 1].y) 
        });
      }
    }

    const xAxis = d3.axisBottom(xScale).ticks(10).tickFormat(d3.format(".0f"));
    const yAxis = d3.axisLeft(yScale).ticks(10).tickFormat(d3.format(".0f"));

    // Add lines
    for (let i = 0; i < lines.length; i++) {
      g.selectAll(`.line-${i}`)
        .data(lines[i])
        .enter()
        .append('line')
        .attr('class', `line line-${i}`)
        .attr('x1', d => d.x1)
        .attr('y1', d => d.y1)
        .attr('x2', d => d.x2)
        .attr('y2', d => d.y2)
        .attr('stroke', colors[i % colors.length]);

      // Add line labels
      svg.append('text')
        .attr('x', dimensions.width - margin.right)
        .attr('y', margin.top + i * 20)
        .attr('text-anchor', 'end')
        .attr('font-size', '12px')
        .attr('fill', colors[i % colors.length])
        .text(labels[i]);
    }
    
    // Add points
    if (!noPoints) {
      g.selectAll('circle')
        .data(allPoints)
        .enter()
        .append('circle')
        .attr('class', 'point')
        .attr('cx', d => xScale(d.x))
        .attr('cy', d => yScale(d.y))
        .attr('r', 5);
    }

    g.append('g')
      .attr('transform', `translate(0, ${height})`)
      .call(xAxis);

    g.append('g')
      .call(yAxis);

    // Add axis labels
    svg.append('text')
      .attr('x', dimensions.width / 2)
      .attr('y', dimensions.height - 20)
      .attr('text-anchor', 'middle')
      .attr('font-size', '12px')
      .attr('fill', 'white')
      .text(xName);

    svg.append('text')
      .attr('x', -dimensions.height / 2)
      .attr('y', 15)
      .attr('text-anchor', 'middle')
      .attr('font-size', '12px')
      .attr('fill', 'white')
      .attr('transform', 'rotate(-90)')
      .text(yName);
      
  }, [dimensions, graphs, noPoints, sorted, xName, yName, labels]);

  return (
    <div ref={containerRef} className="Graph"  style={{ width: '100%'}}>
      <svg 
        ref={svgRef} 
      ></svg>
    </div>
  );
};

export default Graph;