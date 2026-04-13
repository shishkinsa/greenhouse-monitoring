import { App as AntApp, ConfigProvider } from 'antd';
import ruRU from 'antd/locale/ru_RU';
import type { ReactNode } from 'react';
import theme from '@/shared/config/theme';

/**
 * Глобальные провайдеры: тема и локаль Ant Design, контекст для `message` / `notification` (`App`).
 */
export function AppProviders({ children }: { children: ReactNode }) {
  return (
    <ConfigProvider theme={theme} locale={ruRU}>
      <AntApp>{children}</AntApp>
    </ConfigProvider>
  );
}
