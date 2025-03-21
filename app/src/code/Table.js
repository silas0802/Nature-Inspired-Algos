import React from 'react';

const Table = ({ bitEntries, labels }) => {
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
          {(() => {
            // Find the maximum number of iterations across all algorithms
            const maxIterations = Math.max(
              ...bitEntries.map((algorithm) => algorithm.length)
            );

            // Create an array of indices from 0 to maxIterations-1
            return Array.from(
              { length: maxIterations },
              (_, iterationIndex) => (
                <tr key={iterationIndex}>
                  <td>{iterationIndex}</td>
                  {bitEntries.map(
                    (algorithmData, algorithmIndex) => (
                      <td key={algorithmIndex}>
                        {algorithmData[iterationIndex] ? algorithmData[iterationIndex].join("") : ""}
                      </td>
                    )
                  )}
                </tr>
              )
            );
          })()}
        </tbody>
    </table>
  );
};

export default Table;