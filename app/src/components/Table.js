import React, { useState } from 'react';

const Table = ({ rows, labels, stepCount, firstColName }) => {
  const [isReversed, setIsReversed] = useState(false);

  const toggleReverse = () => {
    setIsReversed(!isReversed);
  };

  const downloadCSV = () => {
    // Create headers row
    const headers = [firstColName, ...labels].join(',');
    
    // Process data rows - reverse if needed
    const displayedRows = rows.slice(0, stepCount || rows.length);
    const csvRows = displayedRows.map(row => row.join(','));
    
    // Combine into CSV content
    const csvContent = [headers, ...csvRows].join('\n');
    
    // Create file and trigger download
    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
    const url = URL.createObjectURL(blob);
    const link = document.createElement('a');
    
    // Set link properties
    link.setAttribute('href', url);
    link.setAttribute('download', `table_data_${new Date().toISOString().slice(0,10)}.csv`);
    link.style.visibility = 'hidden';
    
    // Add to DOM, trigger click, and remove
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  };

  return (
    <div className="table-container">
        <table className="bit-table">
          <thead>
            <tr>
              <th>
                {firstColName}
                <button 
                  onClick={toggleReverse} 
                  style={{marginLeft: 5, marginRight: 5, cursor: 'pointer'}}
                  title={isReversed ? "Switch to ascending" : "Switch to descending"}
                >
                  {isReversed ? '↑' : '↓'}
                </button>
                <button onClick={downloadCSV} style={{marginLeft: 5}}>Download CSV</button>
              </th>
              {labels.map((label, index) => (
                <th key={index}>{label}</th>
              ))}
            </tr>
          </thead>
          <tbody>
            {rows.slice(0, stepCount || rows.length)
              .slice()  // Create a copy to avoid mutating the original array
              [isReversed ? 'reverse' : 'slice']()  // Conditionally reverse
              .map((row, rowIndex) => (
                <tr key={rowIndex}>
                  {row.map((cell, cellIndex) => (
                    <td key={cellIndex}>{cell}</td>
                  ))}
                </tr>
              ))}
          </tbody>
        </table>
      </div>
  );
};

export default Table;