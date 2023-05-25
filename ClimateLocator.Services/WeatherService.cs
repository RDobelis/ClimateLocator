using ClimateLocator.Core.Interfaces;
using ClimateLocator.Core.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ClimateLocator.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public WeatherService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration.GetSection("WeatherApiKey").Value;
        }

        public async Task<Weather> GetWeatherAsync(Location location)
        {
            Weather weather = null;

            var url = $"https://api.weatherbit.io/v2.0/current?lat={location.Latitude}&lon={location.Longitude}&key={_apiKey}";

            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(content);
                weather = weatherResponse.Data.FirstOrDefault();
            }

            return weather;
        }
    }
}
