using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt_RESTfulWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            WeatherForecast[] weatherForecasts = new WeatherForecast[]
            {
                WeatherForecast.GetRandomForecast(),
                WeatherForecast.GetRandomForecast(),
                WeatherForecast.GetRandomForecast(),
                WeatherForecast.GetRandomForecast(),
                WeatherForecast.GetRandomForecast(),
            };

            return weatherForecasts;
        }
    }
}
