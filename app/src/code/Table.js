import React from 'react';

const Table = ({ rows, labels, stepCount }) => {
  return (
    <table className="bit-table">
      <thead>
        <tr>
          <th>Step</th>
          {labels.map((label, index) => (
            <th key={index}>{label}</th>
          ))}
        </tr>
      </thead>
      <tbody>
        {rows.slice(0, stepCount || rows.length).map((row, rowIndex) => (
          <tr key={rowIndex}>
            {row.map((cell, cellIndex) => (
              <td key={cellIndex}>{cell}</td>
            ))}
          </tr>
        ))}
      </tbody>
    </table>
  );
};

export default Table;