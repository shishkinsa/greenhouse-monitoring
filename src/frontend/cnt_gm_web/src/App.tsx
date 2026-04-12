import {BrowserRouter, Routes, Route } from 'react-router-dom'
import AppLayout from './components/AppLayout';
import WeatherPage from './pages/WatherPage';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<AppLayout />}>
          <Route path="weather" element={<WeatherPage />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App
