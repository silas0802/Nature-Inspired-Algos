// filepath: /c:/Users/silas/Documents/Hobby Projects/Mackrill/app/src/js/Home.js
import React from 'react';
import Graph from './Graph';
import '../style/BitPage.css';
import BitDiagram from './BitDiagram';

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
  const bitsEntries = [
    [0,0,0,0],
    [0,0,0,1],
    [0,0,1,0],
    [0,0,1,1],
    [0,1,0,0],
    [0,1,0,1],
    [0,1,1,0],
    [0,1,1,1],
    [1,0,0,0],
    [1,0,0,1],
    [1,0,1,0],
    [1,0,1,1],
    [1,1,0,0],
    [1,1,0,1],
    [1,1,1,0],
    [1,1,1,1],
  ];

  return (
    <div>
      <h1>Bit Strings</h1>
      <div id="content"> 
        <div id="parameters">
          <h2>Parameters</h2>
          <label htmlFor="bitAmount">Bit Amount:</label>
          <input type="number" id="bitAmount" placeholder="8" />
          <label htmlFor="problem">Problem:</label>
          <select id="problem">
            <option value="OneMax">OneMax</option>
            <option value="LeadingOnes">Leading Ones</option>
          </select>
          <div id="algorithms">
            <p>Algorithms:</p>
            <label>
              <input type="checkbox" id="ea" checked/>
              (1+1) EA
            </label>
            <label>
              <input type="checkbox" id="rls" />
              RLS
            </label>
          </div>
          <button id="run">Run</button>
        </div>
        <BitDiagram bitEntries={bitsEntries} />
        <Graph graphs={[graph1, graph2, graph3]} xName={"Iterations"} yName={"Distance"} labels={["Simulated Anealing", "Ant Colony Simulation", "Evolutiony Algorithm"]} sorted/>
      </div>
    </div>
  );
};

export default BitPage;