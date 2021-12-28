import React from 'react';
import { NavMenu } from './NavMenu';

export function Layout({ children }) {
  return (
    <div>
      <NavMenu />
      <div className="contents bg-gray-100">
        {children}
      </div>
    </div>
  );
}
