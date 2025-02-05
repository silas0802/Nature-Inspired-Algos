import React from 'react';
import CoordinateSystem from './CoordinateSystem';

const TSPPage = () => {
  const points = [
    { x: 50, y: 50 },
    { x: 150, y: 150 },
    { x: 250, y: 150 },
    { x: 350, y: 250 },
    { x: 450, y: 350 },
    { x: 50, y: 300 },
  ];

  return (
    <div>
      <h1>Travelling Salesman Problem</h1>
      <div className="CoordinateSystem">
        <CoordinateSystem points={points} />
      </div>
    </div>
  );
};

export default TSPPage;