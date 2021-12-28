import React, {useContext, useEffect, useState} from 'react';
import {getData, postData} from "../Api";
import { UserContext } from '../auth/UserContext';
import { parseISO, format } from 'date-fns'
import { Chrono } from "react-chrono";

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
    <div className="d-flex p-2 flex-column align-items-center">
      <input
        className="input-line p-2 m-2 mb-4"
        type="text"
        value={line}
        placeholder="What's today's line?"
        onChange={e => { setLine(e.target.value); }}
        onKeyDown={submitLine}
      />

      {/* To build a custom component of later (https://github.com/prabhuignoto/react-chrono) */}
      <Chrono
        items={lines}
        mode="VERTICAL"
        allowDynamicUpdate="true"
        hideControls="true"
        useReadMore="false"
        cardHeight="50"
        theme={{
          primary: "firebrick",
          secondary: "black",
          cardForeColor: "black",
          titleColor: "white"
        }}
      />
      
    </div>
  );
}
