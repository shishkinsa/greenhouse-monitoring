import {BrowserRouter, Routes, Route } from 'react-router-dom'
import WatherPage from './pages/WatherPage';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<WatherPage/>} />
      </Routes>
    </BrowserRouter>
  );
}

export default App
