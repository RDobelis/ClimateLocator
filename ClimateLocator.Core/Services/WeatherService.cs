using ClimateLocator.Core.Interfaces;
using ClimateLocator.Core.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Polly;
using Polly.Extensions.Http;

namespace ClimateLocator.Core.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly IAsyncPolicy<HttpResponseMessage> _retryPolicy;
        private readonly IMemoryCache _memoryCache;

        public WeatherService(HttpClient httpClient, IConfiguration configuration, IMemoryCache memoryCache)
        {
            _httpClient = httpClient;
            _apiKey = configuration["WeatherApiKey"];

            _retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .RetryAsync(3);

            _memoryCache = memoryCache;
        }

        public async Task<WeatherInfo> GetWeatherInfo(double latitude, double longitude)
        {
            var cacheKey = $"WeatherInfo-{latitude}-{longitude}";
            if (!_memoryCache.TryGetValue(cacheKey, out WeatherInfo cachedWeatherInfo))
            {
                var url = $"https://api.weatherbit.io/v2.0/current?lat={latitude}&lon={longitude}&key={_apiKey}";
                var response = await _retryPolicy.ExecuteAsync(() => _httpClient.GetAsync(url));
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                cachedWeatherInfo = JsonConvert.DeserializeObject<WeatherInfo>(content);

                _memoryCache.Set(cacheKey, cachedWeatherInfo, TimeSpan.FromMinutes(5));
            }
            
            return cachedWeatherInfo;
        }
    }
}
