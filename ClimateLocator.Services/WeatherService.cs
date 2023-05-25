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
        private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;
        private readonly ClimateLocatorDbContext _context;

        public WeatherService(HttpClient httpClient, IConfiguration configuration, ClimateLocatorDbContext context)
        {
            _httpClient = httpClient;
            _apiKey = configuration.GetSection("WeatherApiKey").Value;
            _context = context;

            _circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(3, TimeSpan.FromMinutes(1));
        }

        public async Task<Weather> GetWeatherAsync(Location location)
        {
            Weather weather = null;

            try
            {
                await _circuitBreakerPolicy.ExecuteAsync(async () =>
                {
                    var url =
                        $"https://api.weatherbit.io/v2.0/current?lat={location.Latitude}&lon={location.Longitude}&key={_apiKey}";

                    var response = await _httpClient.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(content);
                        weather = weatherResponse.Data.FirstOrDefault();

                        if (weather != null)
                        {
                            weather.LocationId = location.Id;
                            _context.Weather.Add(weather);
                            await _context.SaveChangesAsync();
                        }
                    }
                });
            }
            catch (BrokenCircuitException)
            {
                throw new WeatherServiceUnavailableException();
            }

            return weather;
        }
    }
}
