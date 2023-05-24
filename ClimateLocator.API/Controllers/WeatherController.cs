using ClimateLocator.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClimateLocator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherService _weatherService;

        public WeatherController(WeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWeatherDataByIp(string ip)
        {
            var weatherData = await _weatherService.GetWeatherDataByIp(ip);

            if (weatherData == null)
                return NotFound();

            return Ok(weatherData);
        }
    }
}
