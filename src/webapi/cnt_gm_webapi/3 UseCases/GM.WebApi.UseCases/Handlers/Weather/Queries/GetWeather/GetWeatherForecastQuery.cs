using GM.WebApi.UseCases.Handlers.WeatherForecast.Queries.GetWeatherForecast.Responses;

using Requestum.Contract;

namespace GM.WebApi.UseCases.Handlers.WeatherForecast.Queries.GetWeather;

public class GetWeatherForecastQuery : IQuery<WeatherForecastResponse>
{
    public int Id { get; set; }
}
