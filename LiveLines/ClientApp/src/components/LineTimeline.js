import React from "react";
import {CSSTransition, TransitionGroup} from 'react-transition-group';
import './timeline.css';
import { format } from 'date-fns'

export function LineTimeline({ lines }) {

  const dateHtml = (year, month) => (
    <CSSTransition key={month+year} timeout={200} classNames="timeline">
      <h1 className="p-1 py-3 font-serif text-xl sm:text-2xl">{month} {year}</h1>
    </CSSTransition>
  );

  const linesAsTimeline = inputLines => {
    let currentYear = '';
    let currentMonth = '';

    let timelineLines = [];
    for (const line of inputLines) {
      const linesYear = format(line.createdAt, 'yyyy');
      const linesMonth = format(line.createdAt, 'MMMM');

      if (linesYear !== currentYear || linesMonth !== currentMonth) {
        timelineLines.push(dateHtml(linesYear, linesMonth));
        currentYear = linesYear;
        currentMonth = linesMonth;
      }

      timelineLines.push(lineDataToHtml(line));
    }

    return timelineLines;
  };
  
  const lineDataToHtml = ({ id, createdAt, message }) => {
      const formattedCreationDate = format(createdAt, 'do');
      return (
        <CSSTransition key={id} timeout={200} classNames="timeline">
        <div className="mx-1 sm:mx-5 flex items-stretch">
            <div className="inline-flex items-center rounded">
              <span className="createdDate text-center bg-white px-3 py-1 border border-gray-200 rounded-xl whitespace-nowrap">{formattedCreationDate}</span>
              <div className="h-1 w-1.5 bg-blue-300" />
            </div>
            <div className="pr-1.5 mr-3 bg-blue-300"/>
            <div className="max-w-prose px-5 py-2 m-2 border border-slate-300 bg-white text-lg">
              <span>{message}</span>
            </div>
        </div>
        </CSSTransition>
    )};
  
  return (
    <div>
      <TransitionGroup>
          {linesAsTimeline(lines)}
      </TransitionGroup>
    </div>
  );
}