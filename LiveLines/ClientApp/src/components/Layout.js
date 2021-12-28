import React from 'react';
import { NavMenu } from './NavMenu';

export function Layout({ children }) {
  return (
    <div className="bg-gray-50 h-full">
      <NavMenu />
      <div className="contents">
        {children}
      </div>
    </div>
  );
}
