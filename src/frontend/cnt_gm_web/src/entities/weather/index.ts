/** Публичный API слайса «погода»: тип, запрос и таблица. */
export type { WeatherForecast } from './model/types';
export { fetchWeatherForecast } from './api/fetchWeatherForecast';
export { WeatherTable } from './ui/WeatherTable';
export type { WeatherTableProps } from './ui/WeatherTable';
