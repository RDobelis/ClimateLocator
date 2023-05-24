using ClimateLocator.Core.Interfaces;
using ClimateLocator.Core.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Polly;
using Polly.Extensions.Http;

namespace ClimateLocator.Core.Services
{
    public class GeolocationService : IGeolocationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly IAsyncPolicy<HttpResponseMessage> _retryPolicy;
        private readonly IMemoryCache _memoryCache;

        public GeolocationService(HttpClient httpClient, IConfiguration configuration, IMemoryCache memoryCache)
        {
            _httpClient = httpClient;
            _apiKey = configuration["GeolocationApiKey"];

            _retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .RetryAsync(3);

            _memoryCache = memoryCache;
        }

        public async Task<GeolocationInfo> GetGeolocationInfo(string ipAddress)
        {
            var cacheKey = $"GeolocationInfo-{ipAddress}";

            if (!_memoryCache.TryGetValue(cacheKey, out GeolocationInfo cachedGeolocationInfo))
            {
                var url = $"https://api.ip2location.io/?key={_apiKey}&ip={ipAddress}";
                var response = await _retryPolicy.ExecuteAsync(() => _httpClient.GetAsync(url));
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                cachedGeolocationInfo = JsonConvert.DeserializeObject<GeolocationInfo>(content);

                _memoryCache.Set(cacheKey, cachedGeolocationInfo, TimeSpan.FromMinutes(5));
            }

            return cachedGeolocationInfo;
        }
    }
}
