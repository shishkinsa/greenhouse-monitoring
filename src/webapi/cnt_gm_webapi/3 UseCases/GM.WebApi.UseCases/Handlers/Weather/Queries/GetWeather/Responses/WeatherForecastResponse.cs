using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GM.WebApi.UseCases.Handlers.WeatherForecast.Queries.GetWeatherForecast.Dto;

namespace GM.WebApi.UseCases.Handlers.WeatherForecast.Queries.GetWeatherForecast.Responses;

public class WeatherForecastResponse
{
    public WeatherForecastDto[] WeatherForecast { get; set; }
}
