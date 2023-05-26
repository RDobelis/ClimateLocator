using ClimateLocator.Core.Exceptions;
using ClimateLocator.Core.Interfaces;
using ClimateLocator.Core.Models;
using ClimateLocator.Data;
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
        private readonly string _apiUrl;
        private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;
        private readonly ClimateLocatorDbContext _context;

        public WeatherService(HttpClient httpClient, IConfiguration configuration, ClimateLocatorDbContext context)
        {
            _httpClient = httpClient;
            _apiKey = configuration.GetSection("WeatherApiKey").Value;
            _apiUrl = configuration.GetSection("WeatherApiUrl").Value;
            _context = context;

            _circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(3, TimeSpan.FromMinutes(1));
        }

        public async Task<Weather> GetWeatherAsync(Location location)
        {
            try
            {
                return await _circuitBreakerPolicy.ExecuteAsync(async () =>
                {
                    return await FetchAndStoreWeatherData(location);
                });
            }
            catch (BrokenCircuitException)
            {
                throw new WeatherServiceUnavailableException();
            }
        }

        private async Task<Weather> FetchAndStoreWeatherData(Location location)
        {
            var weatherData = await GetWeatherFromAPI(location);

            if (weatherData != null)
            {
                weatherData.LocationId = location.Id;
                _context.Weather.Add(weatherData);
                await _context.SaveChangesAsync();
            }

            return weatherData;
        }

        private async Task<Weather> GetWeatherFromAPI(Location location)
        {
            var url = $"{_apiUrl}current?lat={location.Latitude}&lon={location.Longitude}&key={_apiKey}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            var weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(content);

            return weatherResponse.Data.FirstOrDefault();
        }
    }
}
