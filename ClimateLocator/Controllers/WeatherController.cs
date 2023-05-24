using System.Net;
using ClimateLocator.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClimateLocator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherForecastService _weatherForecastService;

        public WeatherController(IWeatherForecastService weatherForecastService)
        {
            _weatherForecastService = weatherForecastService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWeather(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
            {
                return BadRequest("Invalid IP address");
            }

            var weatherInfo = await _weatherForecastService.GetWeatherForecast(ipAddress);

            if (weatherInfo == null)
            {
                return StatusCode((int)HttpStatusCode.ServiceUnavailable, "Weather forecast service unavailable.");
            }

            return Ok(weatherInfo);
        }
    }
}
