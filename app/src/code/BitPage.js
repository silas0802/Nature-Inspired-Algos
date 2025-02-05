// filepath: /c:/Users/silas/Documents/Hobby Projects/Mackrill/app/src/js/Home.js
import React from 'react';
import Graph from './Graph';

const BitPage = () => {
  const graph1 = [
    { x: 50, y: 50 },
    { x: 150, y: 150 },
    { x: 250, y: 150 },
    { x: 350, y: 250 },
    { x: 450, y: 350 },
    { x: 500, y: 550 },
    { x: 750, y: 450 },
    { x: 800, y: 50 },
    { x: 900, y: 700 },

  ];

  return (
    <div>
      <h1>Bit Strings</h1>
      <Graph graphs={[graph1]} />
    </div>
  );
};

export default BitPage;