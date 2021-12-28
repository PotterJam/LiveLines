import React, { useContext, useEffect, useState } from 'react';
import { getData, postData } from "../Api";
import { UserContext } from '../auth/UserContext';
import { LineTimeline } from "../components/LineTimeline";
import { parseISO, format } from 'date-fns'

export function Home() {
  const [line, setLine] = useState("");
  const [lines, setLines] = useState([]);
  const { user } = useContext(UserContext);

  const FormatLineForTimeline = ({ message, createdAt }) => {
    const date = parseISO(createdAt);
    return {
      title: format(date, 'do MMMM yy'),
      cardDetailedText: message
    };
  }

  useEffect(() => {
    const getLines = async () => {
      const resp = await getData("api/lines");
      const linesResp = await resp.json();
      const timelineLines = linesResp.map(FormatLineForTimeline)
      setLines(timelineLines);
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
    const formattedLine = FormatLineForTimeline(newLine);
    
    setLines([formattedLine, ...lines]);
    setLine("");
  }

  return (
    <div className="flex mt-4 p-2 flex-col items-center">
      <input
        className="border-2 border-slate-500 rounded w-4/5 md:w-3/5 lg:w-1/2 xl:w-2/5 text-3xl p-3 m-2 mb-4"
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
}
