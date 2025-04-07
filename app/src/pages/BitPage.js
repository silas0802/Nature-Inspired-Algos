import React, { useState } from 'react';
import Graph from '../components/Graph';
import Table from '../components/Table';
import '../style/BitPage.css';
import BitDiagram from '../components/BitDiagram';

const BitPage = () => {
  const [eaChecked, setEaChecked] = useState(true);
  const [rlsChecked, setRlsChecked] = useState(false);
  const [mmasChecked, setMmasChecked] = useState(false);
  const [graphs, setGraphs] = useState([]);
  const [tableData, setTableData] = useState([]);
  const [bitEntries, setBitEntries] = useState([]);
  const [dataLoaded, setDataLoaded] = useState(false);
  const [labels, setLabels] = useState([]);
  const [stepCount, setStepCount] = useState(0);
  const [expType, setExpType] = useState(0);

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
  function performancesToGraphs(perflist){
    let graphs = [];
    for (let i = 0; i < perflist.length; i++) { //For each algorithm
      const algorithmResults = perflist[i];
      let graph = [];
      for (let j = 0; j < algorithmResults.length; j++) {
        const result = algorithmResults[j]; // The average result at that problem size
        const problemSize = document.getElementById('bitAmount').value/(algorithmResults.length-1) * j;
        graph.push({x: problemSize, y: result});
      }
      graphs.push(graph);
    }
    return graphs;
  }

  function bitsToTable(bitlist){
    var tableData = [];
    const maxIterations = Math.max(...bitlist.map((algorithm) => algorithm.length));
    for (let i = 0; i < maxIterations; i++) { //For each iteration
      let row = [];
      row.push(i);
      for (let j = 0; j < bitlist.length; j++) { //For each algorithm
        if (i < bitlist[j].length){
          row.push(bitlist[j][i].join(""));
        }
        else{
          row.push("");
        }
      }

      tableData.push(row);
    }
    return tableData;
  }

  function performancesToTable(perflist){
    var tableData = [];
    const steps = perflist[0].length;
    for (let i = 0; i < steps; i++) { //For each step
      let row = [];
      const problemSize = Math.floor(document.getElementById('bitAmount').value/(steps-1) * (i));
      row.push(problemSize);
      for (let j = 0; j < perflist.length; j++) { //For each algorithm
        row.push(perflist[j][i]); // in the row add the performance at that step for each algorithm
      }
      tableData.push(row);
    }
    console.log(tableData);
    return tableData;
  }
  
  function getAlgorithmBits() {
    let val = 0;
    if (eaChecked) {
      val += 1;
    }
    if (rlsChecked) {
      val += 2;
    }
    if (mmasChecked) {
      val += 4;
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
    if (mmasChecked) {
      labels.push("MMAS");
    }
    return labels;
  }
  function resetExperiment(event){
    const newExpType = parseInt(event.target.value); // Get the new value directly
    setExpType(newExpType);
    setStepCount(0);
    setDataLoaded(false);
    setBitEntries([]);
    setGraphs([]);
    setLabels([]);
  }

  async function handleStepByStep(bitAmount, algorithms, problem){
    if (stepCount === 0){
      handleDetailedRunComparison(bitAmount, algorithms, problem);
    }
    setStepCount(stepCount + 1);
  }
  async function handleDetailedRunComparison(bitAmount, algorithms, problem){
    try {
      setLabels(getLabels());
      const data = await fetchBitStringRun(bitAmount, algorithms, problem );
      // Handle the response data as needed
      console.log(data);
      setGraphs(bitsToGraphs(data));
      setTableData(bitsToTable(data));
      setBitEntries(data);
      setDataLoaded(true);
      
    } catch (error) {
      console.error('There was a problem with the fetch operation:', error);
      
    }
  }
  async function handlePerformanceComparison(bitAmount, expCount, expSteps, algorithms, problem){
    try {
      setLabels(getLabels());
      const data = await fetchBitStringExp(bitAmount, expCount, expSteps, algorithms, problem);
      // Handle the response data as needed
      console.log(data);
      setGraphs(performancesToGraphs(data));
      setTableData(performancesToTable(data));
      setBitEntries(data);
      setDataLoaded(true);
      
    } catch (error) {
      console.error('There was a problem with the fetch operation:', error);
      
    }
  }

  const handleRunClick = async () => {
    const experimentType = document.getElementById('experimentType').value;
    const bitAmount = document.getElementById('bitAmount').value;
    const problem = document.getElementById('problem').value;
    const algorithms = getAlgorithmBits();
    
    switch (parseInt(experimentType)) {
      case 0: //step by step
        await handleStepByStep(bitAmount, algorithms, problem);
        break;
      case 1: //detailed run comparison
        await handleDetailedRunComparison(bitAmount, algorithms, problem);
        break;
      case 2: //performance comparison
        const expCount = document.getElementById('expCount').value;
        const expSteps = document.getElementById('expSteps').value;
        await handlePerformanceComparison(bitAmount, expCount, expSteps, algorithms, problem);
        break;
    
      default:
        break;
    }
  };

  async function fetchBitStringRun(bitAmount, algorithms, problem) {
    if (!bitAmount || bitAmount <= 0 || bitAmount > 1000) {
      alert('Bit amount must be between 1 and 1000');
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

    const url = `https://localhost:7143/Bit/BitstringRun?problemSize=${bitAmount}&algorithmI=${algorithms}&problemI=${problem}`;
    // Fetch data from the server
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
    return data;
  }

  async function fetchBitStringExp(bitAmount, expCount, expSteps, algorithms, problem) {
    if (!bitAmount || bitAmount <= 0 || bitAmount > 1000) {
      alert('Bit amount must be between 1 and 1000');
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

    const url = `https://localhost:7143/Bit/BitstringExp?maxProblemSize=${bitAmount}&expCount=${expCount}&expSteps=${expSteps}&algorithmI=${algorithms}&problemI=${problem}`;
    // Fetch data from the server
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
    return data;
  } 


  return (
    <div>
      <h1>Bit Strings</h1>
      <div id="content"> 
        <div id="topGrid">
          <div id="parameters">
            <h2>Parameters</h2>
            <label htmlFor="experimentType">Experiment Type:</label>
            <select id="experimentType" onChange={resetExperiment}>
              <option value="0">Step by step</option>
              <option value="1">Detailed run comparison</option>
              <option value="2">Performance comparison</option>
            </select>
            <label htmlFor="bitAmount">Problem Size:</label>
            <input type="number" id="bitAmount" defaultValue={8} />
            {parseInt(expType) === 2 && <label htmlFor="expCount">Exp Count:</label>}
            {parseInt(expType) === 2 && <input type="number" id="expCount" defaultValue={5} />}
            {parseInt(expType) === 2 && <label htmlFor="expSteps">Exp Steps:</label>}
            {parseInt(expType) === 2 && <input type="number" id="expSteps" defaultValue={10} />}
            <label htmlFor="problem">Problem:</label>
            <select id="problem">
              <option value="0">OneMax</option>
              <option value="1">Leading Ones</option>
            </select>
            <div id="algorithms">
              <label>Algorithms:</label>
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
              <label>
                <input
                  type="checkbox"
                  id="mmas"
                  checked={mmasChecked}
                  onChange={() => setMmasChecked(!mmasChecked)}
                />
                MMAS
              </label>
            </div>
            <button id="run" onClick={handleRunClick}>Run</button>
            {stepCount > 0 && <button id="newRun" onClick={resetExperiment}>New Run</button>}
          </div>
          {dataLoaded && expType!==2 && <BitDiagram bitEntries={bitEntries} stepCount={stepCount} />}
          {dataLoaded && <Graph 
            stepCount={stepCount}
            graphs={graphs} 
            xName={expType!==2 ? "Iterations" : "Problem Size"} 
            yName={expType!==2 ? "Amount of Ones" : "Average Iterations"} 
            labels={labels} 
            noPoints
            />}
        </div>
        {dataLoaded && <h3>Experiment Data</h3>}
        {dataLoaded && <Table rows={tableData} labels={labels} stepCount={stepCount} />}   
      </div>
    </div>
  );
};

export default BitPage;