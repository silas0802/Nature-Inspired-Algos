// filepath: /c:/Users/silas/Documents/Hobby Projects/Mackrill/app/src/js/App.js
import React from 'react';
import { BrowserRouter as Router, Route, Routes, Link } from 'react-router-dom';
import '../style/App.css';
import TSPPage from './TSPPage';
import BitPage from './BitPage';

function App() {
  

  return (
    <Router>
      <div className="App">
        <header className="App-header">
          <nav className="navbar">
            <ul className="navbar-menu">
              <li className="navbar-item">
                <Link to="/" className="navbar-link">Bit Strings</Link>
              </li>
              <li className="navbar-item">
                <Link to="/TSP" className="navbar-link">Travelling Salesman Problem</Link>
              </li>
            </ul>
          </nav>
          <Routes>
            <Route path="/" element={<BitPage />} />
            <Route path="/TSP" element={<TSPPage />} />
          </Routes>
        </header>
      </div>
    </Router>
  );
}

export default App;