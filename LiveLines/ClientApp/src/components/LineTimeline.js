import React from "react";
import {CSSTransition, TransitionGroup} from 'react-transition-group';
import './timeline.css';

export function LineTimeline({ lines }) {
  
  const LineToHtml = ({ id, createdAt, message }) => (
    <CSSTransition key={id} timeout={200} classNames="timeline">
      <div className="mx-1 sm:mx-5 flex items-stretch">
        <div className="inline-flex items-center rounded bg-gray-50">
          <span className="bg-white px-3 py-1 border border-gray-200 rounded-xl whitespace-nowrap">{createdAt}</span>
          <div className="h-1 w-1.5 bg-blue-300"></div>
        </div>
        <div className="pr-1.5 mr-3 bg-blue-300"/>
        <div className="max-w-prose px-5 py-2 m-2 border border-slate-300 bg-white text-lg">
          <span>{message}</span>
        </div>
      </div>
    </CSSTransition>
  );
  
  return (
    <div>
      <TransitionGroup>
          {lines.map(LineToHtml)}
      </TransitionGroup>
    </div>
  );
}