using ClimateLocator.Data;
using Microsoft.AspNetCore.Mvc;

namespace ClimateLocator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IDataProvider _dataProvider;

        public WeatherController(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetWeather(string ip)
        {
            if (ip == null)
            {
                return BadRequest("Could not determine the IP address of the request originator.");
            }

            var weather = await _dataProvider.GetWeatherAsync(ip);

            if (weather == null)
            {
                return NotFound("Could not find weather data for the specified IP address.");
            }

            return Ok(weather);
        }
    }
}
