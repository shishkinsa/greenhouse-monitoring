import { App } from 'antd';
import { useCallback, useState } from 'react';
import type { WeatherForecast } from '@/entities/weather';
import { fetchWeatherForecast } from '@/entities/weather';

/**
 * Сценарий загрузки прогноза погоды: состояние, вызов API и уведомление об ошибке.
 *
 * Должен использоваться внутри дерева с обёрткой `App` из antd (как в `AppProviders`), иначе `App.useApp()` недоступен.
 *
 * @returns Объект: `data` — последний успешный ответ; `loading` — идёт запрос; `load` — повторить загрузку
 */
export function useLoadWeatherForecast() {
  const { message } = App.useApp();
  const [data, setData] = useState<WeatherForecast[]>([]);
  const [loading, setLoading] = useState(false);

  const load = useCallback(async () => {
    setLoading(true);
    try {
      const rows = await fetchWeatherForecast();
      setData(rows);
    } catch (error) {
      console.error(error);
      message.error('Не удалось загрузить данные!');
    } finally {
      setLoading(false);
    }
  }, [message]);

  return { data, loading, load };
}
