import { useLoadWeatherForecast } from '@/features/weather/load-forecast';
import { WeatherTable } from '@/entities/weather';
import { Button } from 'antd';

/**
 * Страница маршрута `/weather`: заголовок, кнопка загрузки и таблица прогноза.
 */
export function WeatherPage() {
  const { data, loading, load } = useLoadWeatherForecast();

  return (
    <div>
      <h1>Прогноз погоды</h1>
      <Button type="primary" onClick={() => void load()} loading={loading}>
        {loading ? 'Загрузка...' : 'Загрузить данные'}
      </Button>
      <br />
      <WeatherTable data={data} loading={loading} />
    </div>
  );
}
