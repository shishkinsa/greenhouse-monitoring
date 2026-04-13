import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import '@/app/styles/index.css';
import '@/app/styles/ant-overrides.css';
import { AppProviders } from '@/app/providers';
import { AppRouter } from '@/app/router';

/** Точка входа Vite: глобальные стили, провайдеры и роутер. */
createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <AppProviders>
      <AppRouter />
    </AppProviders>
  </StrictMode>,
);
