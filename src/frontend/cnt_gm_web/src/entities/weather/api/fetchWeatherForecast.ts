import { httpJson } from '@/shared/api/http';
import type { WeatherForecast } from '../model/types';

/**
 * Загружает список прогнозов погоды с backend (`/api/weatherforecast/`).
 *
 * @returns Массив записей прогноза
 * @throws {Error} При сетевой ошибке или неуспешном HTTP-статусе (см. `httpJson`)
 */
export async function fetchWeatherForecast(): Promise<WeatherForecast[]> {
  return httpJson<WeatherForecast[]>('/api/weatherforecast/');
}
