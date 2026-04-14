using GM.WebApi.UseCases.Handlers.WeatherForecast.Queries.GetWeatherForecast.Dto;

namespace GM.WebApi.UseCases.Handlers.WeatherForecast.Queries.GetWeatherForecast.Responses;

public class WeatherForecastResponse
{
    public WeatherForecastDto[] WeatherForecast { get; set; }
}
