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
        private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;
        private readonly ClimateLocatorDbContext _context;

        public LocationService(HttpClient httpClient, IConfiguration configuration, ClimateLocatorDbContext context)
        {
            _httpClient = httpClient;
            _apiKey = configuration.GetSection("GeolocationApiKey").Value;
            _context = context;

            _circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(3, TimeSpan.FromMinutes(1));
        }

        public async Task<Location> GetLocationAsync(string ip)
        {
            Location location = null;

            try
            {
                await _circuitBreakerPolicy.ExecuteAsync(async () =>
                {
                    var url = $"https://api.ip2location.io/?key={_apiKey}&ip={ip}";
                    var response = await _httpClient.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var locationResponse = JsonConvert.DeserializeObject<Location>(content);

                        if (locationResponse != null)
                        {
                            var existingLocation = _context.Location.FirstOrDefault(l => l.Ip == locationResponse.Ip);
                            if (existingLocation == null)
                            {
                                _context.Location.Add(locationResponse);
                                await _context.SaveChangesAsync();
                                location = locationResponse;
                            }
                            else
                            {
                                location = existingLocation;
                            }

                            _context.Querries.Add(new Query { Ip = ip });
                            await _context.SaveChangesAsync();
                        }
                    }
                });
            }
            catch (BrokenCircuitException)
            {
                throw new LocationServiceUnavailableException();
            }

            return location;
        }
    }
}
