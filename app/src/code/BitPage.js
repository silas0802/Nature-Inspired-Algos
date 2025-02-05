// filepath: /c:/Users/silas/Documents/Hobby Projects/Mackrill/app/src/js/Home.js
import React from 'react';
import Graph from './Graph';

const BitPage = () => {
  const graph1 = [

    { x: 0, y: 0 },
    { x: 50, y: 50 },
    { x: 1000, y: 550 },
  ];
  const graph2 = [
    { x: 150, y: 150 },
    { x: 250, y: 150 },
    { x: 350, y: 250 },
    { x: 450, y: 350 },
    { x: 500, y: 550 },
    { x: 750, y: 450 },
    { x: 800, y: 50 },
    { x: 900, y: 450 },
  ];
  const graph3 = [
    { x: 0, y: 650 },
    { x: 50, y: 500 },
    { x: 300, y: 400 },
    { x: 600, y: 250 },
    { x: 700, y: 150 },
    { x: 900, y: 100 },

  ];

  return (
    <div>
      <h1>Bit Strings</h1>
      <Graph graphs={[graph1,graph2, graph3]} xName={"Iterations"} yName={"Distance"} labels={["Simulated Anealing", "Ant Colony Simulation", "Evolutiony Algorithm"]} sorted/>
    </div>
  );
};

export default BitPage;