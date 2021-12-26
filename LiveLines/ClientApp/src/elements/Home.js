import React, {useEffect, useState} from 'react';
import {getData, postData} from "../Api";

export function Home() {
  const [line, setLine] = useState("");
  const [lines, setLines] = useState([]);
  
  useEffect(() => {
    const getLines = async () => {
      const linesResp = await getData("api/lines");
      if (linesResp.ok) {
        const lines = await linesResp.json();
        setLines(lines);
      }
    }
    getLines();
  }, []);

  const submitLine = async e => {
    if (e.key !== 'Enter') {
      return;
    }
    
    const newLineResp = await postData("api/line", {
      Message: line
    });
    
    const newLine = await newLineResp.json();

    setLines([newLine, ...lines]);
    setLine("");
  }

  return (
    <div>
      <h1>Welcome to LiveLines</h1>
      <p>Try adding a line.</p>
      <input
        type="text"
        value={line}
        placeholder="Enter today's line"
        onChange={e => { setLine(e.target.value); }}
        onKeyDown={submitLine}
      />
      <ul>
        {lines.map(l => (
          <li key={l.id}>[{l.createdAt}] {l.message}</li>
        ))}
      </ul>
      
    </div>
  );
}
