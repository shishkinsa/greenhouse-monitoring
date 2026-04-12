import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import { ConfigProvider } from 'antd';
import ruRU from 'antd/locale/ru_RU';
import theme from './theme';

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <ConfigProvider theme={theme} locale={ruRU}>
      <App />
    </ConfigProvider>
  </StrictMode>,
)
