import React, { useContext, useEffect, useState } from 'react';
import { getData, postData } from "../Api";
import { UserContext } from '../auth/UserContext';
import { LineTimeline } from "../components/LineTimeline";
import { parseISO } from 'date-fns'

export function Home() {
  const [line, setLine] = useState("");
  const [lines, setLines] = useState([]);
  const { user, loginAttempted } = useContext(UserContext);

  // TODO: Move this kind of logic to a service layer .js file
  const parseLine = line => ({ ...line, createdAt: parseISO(line.createdAt) });

  useEffect(() => {
    const getLines = async () => {
      const resp = await getData("api/lines");
      const linesResp = await resp.json();
      setLines(linesResp.map(parseLine));
    }

    if (user.authenticated) {
        getLines();
    }
  }, [user.authenticated]);

  const submitLine = async e => {
    if (e.key !== 'Enter') {
      return;
    }
    
    const newLineResp = await postData("api/line", { Message: line });
    const newLine = await newLineResp.json();
    
    setLines([parseLine(newLine), ...lines]);
    setLine("");
  }
  
  const linesHtml = (
    <div className="flex flex-col w-11/12 sm:w-4/5 md:w-8/12 lg:w-7/12 xl:w-6/12 2xl:w-2/5">
      <input
        className="whitespace-normal border-2 border-slate-500 rounded text-3xl p-3 m-2 mb-6"
        type="text"
        value={line}
        placeholder="What's today's line?"
        onChange={e => { setLine(e.target.value); }}
        onKeyDown={submitLine}
      />
      <LineTimeline
        lines={lines}
      />
    </div>
  );

  const aboutElement = (
    <div className='flex flex-col w-4/5 sm:w-auto rounded bg-white border p-12'>
      <h1 className='font-serif-header font-bold text-4xl sm:text-5xl text-slate-800 mb-8'>Welcome to LiveLines</h1>
      <span className='pb-3 text-xl'>This is a work in progress.</span>
      <span className='pb-3 text-xl'>The aim is to write a line a day, about anything you want.</span>
      <span className='pb-3 text-xl'>Log in and try it out!</span>
    </div>
  );

  return (
    <div className="h-full flex mt-4 p-2 flex-col items-center">
      {loginAttempted && user.authenticated && linesHtml}
      {loginAttempted && !user.authenticated && aboutElement}
    </div>
  );
}