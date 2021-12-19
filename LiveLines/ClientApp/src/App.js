import React from 'react';
import { Route, Routes } from 'react-router-dom';
import { Layout } from './elements/Layout';
import { Home } from './elements/Home';
import { FetchData } from './elements/FetchData';
import { Counter } from './elements/Counter';

import './custom.css'

export default function App() {
  return (
    <Layout>
        <Routes>
            <Route exact path='/' element={<Home />} />
            <Route exact path='/counter' element={<Counter />} />
            <Route exact path='/fetch-data' element={<FetchData />} />
        </Routes>
    </Layout>
  );
}
