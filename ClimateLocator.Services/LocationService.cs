using ClimateLocator.Core.Interfaces;
using ClimateLocator.Core.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ClimateLocator.Services
{
    public class LocationService : ILocationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public LocationService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration.GetSection("GeolocationApiKey").Value;
        }

        public async Task<Location> GetLocationAsync(string ip)
        {
            var response = await _httpClient.GetAsync($"https://api.ip2location.io/?key={_apiKey}&ip={ip}");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var location = JsonConvert.DeserializeObject<Location>(content);

            return location;
        }
    }
}
