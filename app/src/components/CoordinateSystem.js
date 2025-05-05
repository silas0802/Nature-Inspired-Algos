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
  const margin = { top: 40, right: 40, bottom: 50, left: 40 }; // Fixed margin

  if (dimensions.width === 0 || !points || points.length === 0) return;

  // Find min and max values for scaling
  let xMin = Infinity, xMax = -Infinity, yMin = Infinity, yMax = -Infinity;
  
  // Search through all points to find boundaries
  for (let algoI = 0; algoI < points.length; algoI++) {
    const nodes = points[algoI];
    for (let i = 0; i < nodes.length; i++) {
      xMin = Math.min(xMin, nodes[i].x);
      xMax = Math.max(xMax, nodes[i].x);
      yMin = Math.min(yMin, nodes[i].y);
      yMax = Math.max(yMax, nodes[i].y);
    }
  }
  
  // Create scale functions
  const xScale = d3.scaleLinear()
    .domain([xMin, xMax])
    .range([margin.left, dimensions.width - margin.right]);
    
  const yScale = d3.scaleLinear()
    .domain([yMin, yMax])
    .range([margin.top, dimensions.height - margin.bottom]);

  const svg = d3.select(svgRef.current)
    .attr('width', dimensions.width)
    .attr('height', dimensions.height);

  // Clear previous elements
  svg.selectAll('*').remove();

  // Calculate lines with scaled coordinates
  const lines = [];
  for (let algoI = 0; algoI < points.length; algoI++) {
    const nodes = points[algoI];
    
    // Connect last node to first node to complete the tour
    lines.push({ 
      x1: xScale(nodes[nodes.length - 1].x), 
      y1: yScale(nodes[nodes.length - 1].y), 
      x2: xScale(nodes[0].x), 
      y2: yScale(nodes[0].y), 
      algoI: algoI 
    });
    
    // Add lines between consecutive points
    for (let i = 0; i < nodes.length - 1; i++) {
      lines.push({ 
        x1: xScale(nodes[i].x), 
        y1: yScale(nodes[i].y), 
        x2: xScale(nodes[i+1].x), 
        y2: yScale(nodes[i+1].y), 
        algoI: algoI
      });
    }
  }

  // Add lines (using scaled coordinates)
  svg.selectAll('line')
    .data(lines)
    .enter()
    .append('line')
    .attr('class', d => `line algo${d.algoI}`)
    .attr('x1', d => d.x1)
    .attr('y1', d => d.y1)
    .attr('x2', d => d.x2)
    .attr('y2', d => d.y2)
    .attr('stroke', d => colors[d.algoI % colors.length])
    .attr('stroke-width', d => 5-d.algoI);
  
  // Add labels (unchanged)
  for (let i = 0; i < points.length; i++) {
    svg.append('text')
      .attr('class', `line-label algo${i}`)
      .attr('x', dimensions.width - margin.right)
      .attr('y', margin.top + i * 20)
      .attr('text-anchor', 'end')
      .attr('font-size', '16px')
      .attr('fill', colors[i % colors.length])
      .text(labels[i])
      .style('cursor', 'pointer')
      .on('mouseenter', function() {
        // Dim all lines
        svg.selectAll('line')
          .style('opacity', 0.2);
        
        // Highlight this algorithm's lines
        svg.selectAll(`line.algo${i}`)
          .style('opacity', 1)
          .style('stroke-width', 5-i+2)
          .style('filter', 'drop-shadow(0px 0px 3px rgba(0,0,0,0.5))');
        
        // Emphasize this label
        d3.select(this)
          .style('font-weight', 'bold')
          .style('text-shadow', '1px 1px 2px rgba(0,0,0,0.2)');
      })
      .on('mouseleave', function() {
        // Restore all lines
        svg.selectAll('line')
          .style('opacity', 1)
          .style('stroke-width', d => 5-d.algoI)
          .style('filter', 'none');
        
        // Restore this label
        d3.select(this)
          .style('font-weight', 'normal')
          .style('text-shadow', 'none');
      });
  }

  // Add points using scaled coordinates
  svg.selectAll('circle')
    .data(points[0])
    .enter()
    .append('circle')
    .attr('class', 'point')
    .attr('cx', d => xScale(d.x))
    .attr('cy', d => yScale(d.y))
    .attr('r', 5)
    .attr('fill', 'lightcoral');

}, [points, dimensions, labels]);

  return (
    <div ref={containerRef} className="CoordinateSystem" style={{ width: '100%', height: '100%' }}>
      <svg ref={svgRef}></svg>
    </div>
  );
};

export default CoordinateSystem;