/** Одна строка прогноза погоды в ответе API (совместима с демо WebApi). */
export interface WeatherForecast {
  key?: string;
  date: string;
  temperatureC: number;
  temperatureF?: number;
  summary: string;
}
