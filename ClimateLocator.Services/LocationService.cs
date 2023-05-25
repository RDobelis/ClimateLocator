using ClimateLocator.Core.Interfaces;
using ClimateLocator.Core.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Polly;
using Polly.CircuitBreaker;

namespace ClimateLocator.Services
{
    public class LocationService : ILocationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;

        public LocationService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration.GetSection("GeolocationApiKey").Value;

            _circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(3, TimeSpan.FromMinutes(1));
        }

        public async Task<Location> GetLocationAsync(string ip)
        {
            Location location = null;

            await _circuitBreakerPolicy.ExecuteAsync(async () =>
            {
                var url = $"https://api.ip2location.io/?key={_apiKey}&ip={ip}";
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    location = JsonConvert.DeserializeObject<Location>(content);
                }
            });

            return location;
        }
    }
}
