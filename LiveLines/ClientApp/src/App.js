import React from 'react';
import { Route, Routes } from 'react-router-dom';
import { Layout } from './elements/Layout';
import { Home } from './elements/Home';

import './custom.css'

export default function App() {
  return (
    <Layout>
        <Routes>
            <Route exact path='/' element={<Home />} />
        </Routes>
    </Layout>
  );
}
