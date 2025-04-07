import React, {useState} from 'react';
import CoordinateSystem from '../components/CoordinateSystem';
import '../style/TSPPage.css';
const TSPPage = () => {

  const [eaChecked, setEaChecked] = useState(true);
  const [simAnnealChecked, setSimAnnealChecked] = useState(false);
  const [mmasChecked, setMmasChecked] = useState(false);
  const [expType, setExpType] = useState(0);
  const [stepCount, setStepCount] = useState(0);
  const [dataLoaded, setDataLoaded] = useState(false);
  const [labels, setLabels] = useState([]);
  const [tableData, setTableData] = useState([]);
  const [graphs, setGraphs] = useState([]);
  
  
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

  async function fetchTSPRun(problemSize, algorithms) {
    if (!problemSize || problemSize <= 0 || problemSize > 1000) {
      alert('Bit amount must be between 1 and 1000');
      return;
    }
    if (algorithms === 0){
      alert('At least one algorithm must be selected');
      return;
    }

    const url = `https://localhost:7143/TSP/TSPRun?problemSize=${problemSize}&algorithmI=${algorithms}`;
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

  async function fetchTSPExp(problemSize, expCount, expSteps, algorithms) {
    if (!problemSize || problemSize <= 0 || problemSize > 1000) {
      alert('Bit amount must be between 1 and 1000');
      return;
    }
    if (algorithms === 0){
      alert('At least one algorithm must be selected');
      return;
    }

    const url = `https://localhost:7143/TSP/TSPExp?maxProblemSize=${problemSize}&expCount=${expCount}&expSteps=${expSteps}&algorithmI=${algorithms}`;
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

  function resetExperiment(event){
    const newExpType = parseInt(event.target.value); // Get the new value directly
    setExpType(newExpType);
    setStepCount(0);
    setDataLoaded(false);
    setGraphs([]);
    setLabels([]);
  }

  async function handleStepByStep(problemSize, algorithms){
    if (stepCount === 0){
      handleDetailedRunComparison(problemSize, algorithms);
    }
    setStepCount(stepCount + 1);
  }
  async function handleDetailedRunComparison(problemSize, algorithms){
    try {
      setLabels(getLabels());
      const data = await fetchTSPRun(problemSize, algorithms);
      // Handle the response data as needed
      console.log(data);
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
      console.log(data);
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
        await handleStepByStep(problemSize, algorithms);
        break;
      case 1: //detailed run comparison
        await handleDetailedRunComparison(problemSize, algorithms);
        break;
      case 2: //performance comparison
        const expCount = document.getElementById('expCount').value;
        const expSteps = document.getElementById('expSteps').value;
        await handlePerformanceComparison(problemSize, expCount, expSteps, algorithms);
        break;
    
      default:
        break;
    }
  };

  return (
    <div>
      <h1>Travelling Salesman Problem</h1>
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
            <label htmlFor="problemSize">Problem Size:</label>
            <input type="number" id="problemSize" defaultValue={8} />
            {parseInt(expType) === 2 && <label htmlFor="expCount">Exp Count:</label>}
            {parseInt(expType) === 2 && <input type="number" id="expCount" defaultValue={5} />}
            {parseInt(expType) === 2 && <label htmlFor="expSteps">Exp Steps:</label>}
            {parseInt(expType) === 2 && <input type="number" id="expSteps" defaultValue={10} />}
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
          </div>
          <CoordinateSystem points={[{x: 50,y: 250},{x: 50,y: 150},{x: 450,y: 350}]} labels={labels} />
        </div>
      </div>
    </div>
  );
};

export default TSPPage;