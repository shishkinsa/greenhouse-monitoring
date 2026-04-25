
using GM.WebApi.UseCases.Handlers.WeatherForecast.Queries.GetWeather;
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

        var result = await _requestum.HandleAsync<GetWeatherForecastQuery, WeatherForecastResponse>(new GetWeatherForecastQuery { Id = 1 });

        return Ok(result.WeatherForecast);
    }
}
