import React, { useEffect, useContext } from 'react';
import { Route, Routes } from 'react-router-dom';
import { Layout } from './elements/Layout';
import { Home } from './elements/Home';
import { UserContext } from './auth/UserContext';

import './custom.css'

export default function App() {
  const { login, logout } = useContext(UserContext);

  useEffect(() => {
    const checkIfLoggedIn = async () =>
    {
      const response = await fetch('api/authenticated');
      const { username, authenticated } = await response.json();
      
      if (authenticated) {
        login(username);
      } else {
        logout(); 
      }
    }
    checkIfLoggedIn();
  
    // Disabled warning because we're only using OAuth, on login it will always redirect
    // and cause this to run again if the authentication status changes.
    // If different login schemes are added this assumption may be false.
  }, []); // eslint-disable-line react-hooks/exhaustive-deps

  return (
    <Layout>
      <Routes>
        <Route exact path='/' element={<Home />} />
      </Routes>
    </Layout>
  );
}
