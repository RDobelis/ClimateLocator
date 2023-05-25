using ClimateLocator.Core.Interfaces;
using ClimateLocator.Core.Models;
using Microsoft.Extensions.Caching.Memory;

namespace ClimateLocator.Data
{
    public class DataProvider : IDataProvider
    {
        private readonly ILocationService _locationService;
        private readonly IWeatherService _weatherService;
        private readonly IMemoryCache _cache;

        public DataProvider(ILocationService locationService, IWeatherService weatherService, IMemoryCache cache)
        {
            _locationService = locationService;
            _weatherService = weatherService;
            _cache = cache;
        }

        public async Task<Weather> GetWeatherAsync(string ip)
        {
            if (!_cache.TryGetValue(ip, out Weather weather))
            {
                var location = await _locationService.GetLocationAsync(ip);
                weather = await _weatherService.GetWeatherAsync(location);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));
                _cache.Set(ip, weather, cacheOptions);
            }

            return weather;
        }
    }
}
