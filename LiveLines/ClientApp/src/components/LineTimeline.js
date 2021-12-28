import React from "react";

export function LineTimeline({ lines }) {
  
  const LineToHtml = ({ id, createdAt, message }) => (
    <div key={id} className="ml-5 flex items-center">
      <div className="p-1 m-2 mr-5 rounded bg-gray-50">
        <span className="whitespace-normal sm:whitespace-nowrap">{createdAt}</span>
      </div>
      <div className="max-w-prose px-5 py-2 m-2 rounded-br-lg rounded-bl-lg rounded-tr-lg border border-slate-300 bg-white text-lg">
        <span>{message}</span>
      </div>
    </div>
  );
  
  return (
    <div>
      {lines.map(LineToHtml)}
    </div>
  );
}