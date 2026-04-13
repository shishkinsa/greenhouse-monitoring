import { AppLayout } from '@/widgets/app-layout';
import { WeatherPage } from '@/pages/weather';
import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom';

/**
 * Корневое дерево маршрутов SPA: layout и вложенные страницы.
 */
export function AppRouter() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<AppLayout />}>
          <Route index element={<Navigate to="/weather" replace />} />
          <Route path="weather" element={<WeatherPage />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}
