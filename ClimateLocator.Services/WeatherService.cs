using ClimateLocator.Core.Interfaces;
using ClimateLocator.Core.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Polly;
using Polly.CircuitBreaker;

namespace ClimateLocator.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;

        public WeatherService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration.GetSection("WeatherApiKey").Value;

            _circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(3, TimeSpan.FromMinutes(1));
        }

        public async Task<Weather> GetWeatherAsync(Location location)
        {
            Weather weather = null;

            await _circuitBreakerPolicy.ExecuteAsync(async () =>
            {
                var url = $"https://api.weatherbit.io/v2.0/current?lat={location.Latitude}&lon={location.Longitude}&key={_apiKey}";

                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(content);
                    weather = weatherResponse.Data.FirstOrDefault();
                }
            });

            return weather;
        }
    }
}
