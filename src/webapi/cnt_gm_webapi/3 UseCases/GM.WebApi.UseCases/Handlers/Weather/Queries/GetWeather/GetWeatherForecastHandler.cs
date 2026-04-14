using GM.WebApi.UseCases.Handlers.WeatherForecast.Queries.GetOrders;
using GM.WebApi.UseCases.Handlers.WeatherForecast.Queries.GetWeatherForecast.Dto;
using GM.WebApi.UseCases.Handlers.WeatherForecast.Queries.GetWeatherForecast.Responses;

using Requestum.Contract;

namespace GM.WebApi.UseCases.Handlers.Orders.Queries.GetOrders;

public class GetWeatherForecastHandler: IQueryHandler<GetWeatherForecastQuery, WeatherForecastResponse>
{
    public WeatherForecastResponse Handle(GetWeatherForecastQuery query)
    {
        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecastDto
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();

        return new WeatherForecastResponse
        {
            WeatherForecast = forecast
        };

    }
}
