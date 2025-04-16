import React, {useState} from 'react';
import CoordinateSystem from '../components/CoordinateSystem';
import '../style/TSPPage.css';
import Table from '../components/Table';
import Graph from '../components/Graph';
const TSPPage = () => {

  const BACKEND_URL = 'https://localhost:7143/';

  const [eaChecked, setEaChecked] = useState(true);
  const [simAnnealChecked, setSimAnnealChecked] = useState(false);
  const [mmasChecked, setMmasChecked] = useState(false);
  const [expType, setExpType] = useState(0);
  const [stepCount, setStepCount] = useState(0);
  const [dataLoaded, setDataLoaded] = useState(false);
  const [labels, setLabels] = useState([]);
  const [nodes, setNodes] = useState([]);
  const [solutions, setSolutions] = useState([]);
  const [results, setResults] = useState([]);
  const [diagramPoints, setDiagramPoints] = useState([]);
  const [graphData, setGraphData] = useState([]);
  const [tableData, setTableData] = useState([]);
  
  function getLabels(){
    let labels = [];
    if (eaChecked) {
      labels.push("(1+1) EA");
    }
    if (simAnnealChecked) {
      labels.push("Sim Anneal");
    }
    if (mmasChecked) {
      labels.push("MMAS");
    }
    return labels;
  }

  function getAlgorithmBits() {
    let val = 0;
    if (eaChecked) {
      val += 1;
    }
    if (simAnnealChecked) {
      val += 2;
    }
    if (mmasChecked) {
      val += 4;
    }
    return val;
  }
  function getDiagramPoints(nodes, solutions){
    let allPoints = [];
    const margin = 50; // Margin around the coordinate system
    for (let j = 0; j < solutions.length; j++) {
      const solution = solutions[j];
      let points = [];
      for (let i = 0; i < solution.length; i++) {
        const nodeIndex = solution[i];
        points.push({x: nodes[nodeIndex][0]+margin, y: nodes[nodeIndex][1]});
      }
      allPoints.push(points);
    }
    
    return allPoints;
  }

  function performancesToTable(perflist){
    var tableData = [];
    const steps = perflist[0].length;
    for (let i = 0; i < steps; i++) { //For each step
      let row = [];
      const problemSize = expType === 2 ? Math.floor(document.getElementById('problemSize').value/(steps-1) * (i)) : i;
      row.push(problemSize);
      for (let j = 0; j < perflist.length; j++) { //For each algorithm
        row.push(perflist[j][i]); // in the row add the performance at that step for each algorithm
      }
      tableData.push(row);
    }
    return tableData;
  }
  function resultsToGraphs(results){
    let graphs = [];
    for (let i = 0; i < results.length; i++) { // For each algorithm
      let graph = [];
      for (let j = 0; j < results[i].length; j++) { // For each step
        const problemSize =  Math.floor(document.getElementById('problemSize').value/(results[i].length-1) * (j));
        graph.push({x: expType === 2 ? problemSize : j, y: results[i][j]});
      }
      graphs.push(graph);
    }
    return graphs;
  }

  async function fetchTSPRun(problemSize, algorithms) {
    if (!problemSize || problemSize <= 0 || problemSize > 1000) {
      alert('Bit amount must be between 1 and 1000');
      return;
    }
    if (algorithms === 0){
      alert('At least one algorithm must be selected');
      return;
    }

    const endPointURL = BACKEND_URL+'TSP/TSPRun';
    const parameters = {
        problemSize: problemSize,
        algorithmI: algorithms
    };

    // Fetch data from the server
    const response = await fetch(endPointURL, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(parameters),
        mode: 'cors' // Ensure CORS is enabled
    });
    if (!response.ok) {
      throw new Error('Network response was not ok');
    }
    const data = await response.json();
    return data;
  }

  async function fetchTSPExp(problemSize, expCount, expSteps, algorithms) {
    if (!problemSize || problemSize <= 0 || problemSize > 1000) {
      alert('Bit amount must be between 1 and 1000');
      return;
    }
    if (algorithms === 0){
      alert('At least one algorithm must be selected');
      return;
    }

    const endPointURL = BACKEND_URL+'TSP/TSPExp';
    const parameters = {
        problemSize: problemSize,
        algorithmI: algorithms,
        expCount: expCount,
        expSteps: expSteps
    };

    // Fetch data from the server
    const response = await fetch(endPointURL, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(parameters),
        mode: 'cors' // Ensure CORS is enabled
    });

    if (!response.ok) {
      throw new Error('Network response was not ok');
    }
    const data = await response.json();
    return data;
  } 

  function resetExperiment(event){
    const newExpType = parseInt(event.target.value); // Get the new value directly
    setExpType(newExpType);
    setStepCount(0);
    setDataLoaded(false);
    setLabels([]);
    setNodes([]);
    setSolutions([]);
    setResults([]);
    setTableData([]);
    setGraphData([]);
    setDiagramPoints([]);
  }

  async function handleStepByStep(problemSize, algorithms){
    if (stepCount === 0){
      try {
        setLabels(getLabels());
        const data = await fetchTSPRun(problemSize, algorithms);
        // Handle the response data as needed
        setNodes(data.nodes);
        setSolutions(data.solutions);
        setResults(data.results);
        let bestSols = [];
        for (let i = 0; i < data.solutions.length; i++) {
          const algoBestSol = data.solutions[i][0];
          bestSols.push(algoBestSol);
        }
        setDiagramPoints(getDiagramPoints(data.nodes, bestSols));
        setTableData(performancesToTable(data.results));
        setGraphData(resultsToGraphs(data.results));
        setStepCount(1); // Set stepCount to 1 after the first run
        setDataLoaded(true);
  
        
      } catch (error) {
        console.error('There was a problem with the fetch operation:', error);
        
      }
    }
    else{
      let bestSols = [];
      if (stepCount >= solutions[0].length){
        return;
      }
      for (let i = 0; i < solutions.length; i++) {
        const algoBestSol = solutions[i][stepCount];
        bestSols.push(algoBestSol);
      }
      setDiagramPoints(getDiagramPoints(nodes, bestSols));

      // Look for next step with improvement
      let currentStep = stepCount;
      while (currentStep < results[0].length) {
        for (let i = 0; i < results.length; i++) {
          const algorithmResults = results[i];
          if (algorithmResults[currentStep] !== algorithmResults[stepCount]) { // if we find a better result
            setStepCount(currentStep);
            return;
          }
        }
        currentStep++;
      }
      setStepCount(results[0].length); // If no better result is found, set to last step
    }
    
  }
  async function handleDetailedRunComparison(problemSize, algorithms){
    try {
      setLabels(getLabels());
      const data = await fetchTSPRun(problemSize, algorithms);
      // Handle the response data as needed
      setNodes(data.nodes);
      setSolutions(data.solutions);
      setResults(data.results);
      let bestSols = [];
      for (let i = 0; i < data.solutions.length; i++) {
        const algoBestSol = data.solutions[i][data.solutions[i].length-1];
        bestSols.push(algoBestSol);
      }
      setDiagramPoints(getDiagramPoints(data.nodes, bestSols));
      setTableData(performancesToTable(data.results));
      setGraphData(resultsToGraphs(data.results));
      setDataLoaded(true);

      
    } catch (error) {
      console.error('There was a problem with the fetch operation:', error);
      
    }
  }
  async function handlePerformanceComparison(bitAmount, expCount, expSteps, algorithms){
    try {
      setLabels(getLabels());
      const data = await fetchTSPExp(bitAmount, expCount, expSteps, algorithms);
      // Handle the response data as needed
      setTableData(performancesToTable(data.results));
      setGraphData(resultsToGraphs(data.results));
      setDataLoaded(true);

      
    } catch (error) {
      console.error('There was a problem with the fetch operation:', error);
      
    }
  }

  const handleRunClick = async () => {
    const experimentType = document.getElementById('experimentType').value;
    const problemSize = document.getElementById('problemSize').value;
    const algorithms = getAlgorithmBits();
    
    
    switch (parseInt(experimentType)) {
      case 0: //step by step
        await handleStepByStep(problemSize, algorithms); // Await might not be needed here
        break;
      case 1: //detailed run comparison
        await handleDetailedRunComparison(problemSize, algorithms); // Await might not be needed here
        break;
      case 2: //performance comparison
        const expCount = document.getElementById('expCount').value;
        const expSteps = document.getElementById('expSteps').value;
        await handlePerformanceComparison(problemSize, expCount, expSteps, algorithms); // Await might not be needed here
        break;
    
      default:
        break;
    }
  };

  return (
    <div>
      <h1>Travelling Salesman Problem</h1>
      <div id="content"> 
        <div class="tsp-top-grid">
          <div id="parameters">
            <h2>Parameters</h2>
            <label htmlFor="experimentType">Experiment Type:</label>
            <select id="experimentType" onChange={resetExperiment}>
              <option value="0">Step by step</option>
              <option value="1">Detailed run comparison</option>
              <option value="2">Performance comparison</option>
            </select>
            <label htmlFor="problemSize">Problem Size:</label>
            <input type="number" id="problemSize" defaultValue={8} />
            {parseInt(expType) === 2 && <label htmlFor="expCount">Exp Count:</label>}
            {parseInt(expType) === 2 && <input type="number" id="expCount" defaultValue={5} />}
            {parseInt(expType) === 2 && <label htmlFor="expSteps">Exp Steps:</label>}
            {parseInt(expType) === 2 && <input type="number" id="expSteps" defaultValue={4} />}
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
                  checked={simAnnealChecked}
                  onChange={() => setSimAnnealChecked(!simAnnealChecked)}
                />
                Simulated Annealing
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
            {stepCount > 0 && <button id="newRun" onClick={resetExperiment}>Reset Experiment</button>}
          </div>
          {diagramPoints.length>0 && <CoordinateSystem points={diagramPoints} labels={labels} />}
          {graphData.length > 0 && <Graph graphs={graphData} stepCount={stepCount} labels={labels} xName={expType!==2 ? "Iteration" : "Problem Size"} yName={"Distance"} noPoints/>}
        </div>
        {dataLoaded && <Table rows={tableData} labels={labels} stepCount={stepCount} firstColName={expType!==2 ? "Iteration" : "Problem Size"} />}
      </div>
    </div>
  );
};

export default TSPPage;