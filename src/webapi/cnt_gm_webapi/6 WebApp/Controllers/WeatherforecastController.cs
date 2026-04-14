using System.Collections.Immutable;

using GM.WebApi.UseCases.Handlers.WeatherForecast.Queries.GetOrders;
using GM.WebApi.UseCases.Handlers.WeatherForecast.Queries.GetWeatherForecast.Responses;

using Microsoft.AspNetCore.Mvc;
using Requestum;

namespace GM.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WeatherforecastController: ControllerBase
{
    private readonly IRequestum _requestum;
    public WeatherforecastController(IRequestum requestum)
    {
        _requestum = requestum;
    }

    [HttpGet]
    public async Task<ActionResult<Array>> GetWeather()
    {

        var query = new GetWeatherForecastQuery();

        var result = await _requestum.HandleAsync<GetWeatherForecastQuery, WeatherForecastResponse>(query);

        return Ok(result);
    }
}
