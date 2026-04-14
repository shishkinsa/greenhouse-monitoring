import { AppLayout } from '@/widgets/app-layout';
import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom';
import { HomePage } from '@/pages/home';
import { WeatherPage } from '@/pages/weather';

/**
 * Корневое дерево маршрутов SPA: layout и вложенные страницы.
 */
export function AppRouter() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<AppLayout />}>
          <Route path="/home" element={<HomePage />} />
          <Route path="weather" element={<WeatherPage />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}
