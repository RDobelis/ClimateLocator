using ClimateLocator.Core.Interfaces;
using ClimateLocator.Core.Models;
using Microsoft.Extensions.Caching.Memory;

namespace ClimateLocator.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task<GeolocationInfo> GetGeolocationInfo(string ipAddress)
        {
            return Task.FromResult(_cache.Get<GeolocationInfo>(ipAddress));
        }

        public Task<WeatherInfo> GetWeatherInfo(double latitude, double longitude)
        {
            var key = $"{latitude},{longitude}";
            return Task.FromResult(_cache.Get<WeatherInfo>(key));
        }

        public Task StoreGeolocationInfo(GeolocationInfo geolocationInfo)
        {
            _cache.Set(geolocationInfo.IPAddress, geolocationInfo, TimeSpan.FromMinutes(5));
            return Task.CompletedTask;
        }

        public Task StoreWeatherInfo(WeatherInfo weatherInfo)
        {
            var key = $"{weatherInfo.Latitude},{weatherInfo.Longitude}";
            _cache.Set(key, weatherInfo, TimeSpan.FromMinutes(5));
            return Task.CompletedTask;
        }
    }
}
