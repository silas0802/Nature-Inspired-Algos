import React, { useState } from 'react';
import Graph from './Graph';
import '../style/BitPage.css';
import BitDiagram from './BitDiagram';

const BitPage = () => {
  const [eaChecked, setEaChecked] = useState(true);
  const [rlsChecked, setRlsChecked] = useState(false);
  const [graphs, setGraphs] = useState([[{x: 0, y: 0}, {x: 50, y:50}]]);
  const [bitEntries, setBitEntries] = useState([[[0,1,1,0], [1,1,1,0]],[[0,0,0,0]]]);

  function CountSetBits(bitArray) {
    let count = 0;
    for (let i = 0; i < bitArray.length; i++) {
      count += bitArray[i];
    }
    return count;
  }
  function CountLeadingOnes(bitArray) {
    let count = 0;
    for (let i = 0; i < bitArray.length; i++) {
      if (bitArray[i] === 1) {
        count++;
      } else {
        break;
      }
    }
    return count;
  }

  function bitsToGraphs(bitlist){
    var graphs = [];
    for (let a = 0; a < bitlist.length; a++) { //For each algorithm
      var graph = [];
      for (let j = 0; j < bitlist[a].length; j++) { //For each iteration
        const bitArray = bitlist[a][j];
        let yVal = 0;
        switch (parseInt(document.getElementById('problem').value)) {
          case 0:
            yVal = CountSetBits(bitArray);
            break;
          case 1:
            yVal = CountLeadingOnes(bitArray);
            break;
          default:
            break;          
        }
        
        graph.push({x: j, y: yVal});
      }
      graphs.push(graph);
    }
    return graphs;
  }
  

  
  function getAlgorithmBits() {
    let val = 0;
    if (eaChecked) {
      val += 1;
    }
    if (rlsChecked) {
      val += 2;
    }
    return val;
  }

  function getLabels(){
    let labels = [];
    if (eaChecked) {
      labels.push("(1+1) EA");
    }
    if (rlsChecked) {
      labels.push("RLS");
    }
    return labels;
  }

  const handleRunClick = async () => {
    const bitAmount = document.getElementById('bitAmount').value;
    const problem = document.getElementById('problem').value;
    const algorithms = getAlgorithmBits();

    // Validate input
    if (!bitAmount || bitAmount <= 0 || bitAmount > 64) {
      alert('Bit amount must be between 1 and 64');
      return;
    }
    if (!problem){
      alert('Problem must be selected');
      return;
    }
    if (algorithms === 0){
      alert('At least one algorithm must be selected');
      return;
    }

    const url = `https://localhost:7143/Bit/BitstringRun?maxProblemSize=${bitAmount}&algorithmI=${algorithms}&problemI=${problem}`;
    // Fetch data from the server
    try {
      const response = await fetch(url, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
        },
        mode: 'cors', // Ensure CORS is enabled
      });

      if (!response.ok) {
        throw new Error('Network response was not ok');
      }

      const data = await response.json();
      // Handle the response data as needed
      console.log(data);
      setGraphs(bitsToGraphs(data));
      setBitEntries(data);
    } catch (error) {
      console.error('There was a problem with the fetch operation:', error);
    }
  };

  return (
    <div>
      <h1>Bit Strings</h1>
      <div id="content"> 
        <div id="parameters">
          <h2>Parameters</h2>
          <label htmlFor="bitAmount">Bit Amount:</label>
          <input type="number" id="bitAmount" />
          <label htmlFor="problem">Problem:</label>
          <select id="problem">
            <option value="0">OneMax</option>
            <option value="1">Leading Ones</option>
          </select>
          <div id="algorithms">
            <p>Algorithms:</p>
            <label>
              <input
                type="checkbox"
                id="ea"
                checked={eaChecked}
                onChange={() => setEaChecked(!eaChecked)}
              />
              (1+1) EA
            </label>
            <label>
              <input
                type="checkbox"
                id="rls"
                checked={rlsChecked}
                onChange={() => setRlsChecked(!rlsChecked)}
              />
              RLS
            </label>
          </div>
          <button id="run" onClick={handleRunClick}>Run</button>
        </div>
        <BitDiagram bitEntries={bitEntries} />
        <Graph 
          graphs={graphs} 
          xName={"Iterations"} 
          yName={ "Amount of Ones"} 
          labels={getLabels()} 
          noPoints/>
      </div>
    </div>
  );
};

export default BitPage;