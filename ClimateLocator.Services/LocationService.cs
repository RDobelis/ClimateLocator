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
    public class LocationService : ILocationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _apiUrl;
        private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;
        private readonly ClimateLocatorDbContext _context;

        public LocationService(HttpClient httpClient, IConfiguration configuration, ClimateLocatorDbContext context)
        {
            _httpClient = httpClient;
            _apiKey = configuration.GetSection("GeolocationApiKey").Value;
            _apiUrl = configuration.GetSection("GeolocationApiUrl").Value;
            _context = context;

            _circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(3, TimeSpan.FromMinutes(1));
        }

        public async Task<Location> GetLocationAsync(string ip)
        {
            try
            {
                return await _circuitBreakerPolicy.ExecuteAsync(async () => await GetLocationData(ip));
            }
            catch (BrokenCircuitException)
            {
                throw new LocationServiceUnavailableException();
            }
        }

        private async Task<Location> GetLocationData(string ip)
        {
            var existingLocation = _context.Location.FirstOrDefault(l => l.Ip == ip);

            if (existingLocation != null)
                return existingLocation;

            var locationResponse = await GetExternalLocationData(ip);

            if (locationResponse != null)
            {
                _context.Location.Add(locationResponse);
                await _context.SaveChangesAsync();
            }

            _context.Querries.Add(new Query { Ip = ip });
            await _context.SaveChangesAsync();

            return locationResponse;
        }

        private async Task<Location> GetExternalLocationData(string ip)
        {
            var url = $"{_apiUrl}?key={_apiKey}&ip={ip}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Location>(content);
        }
    }
}
