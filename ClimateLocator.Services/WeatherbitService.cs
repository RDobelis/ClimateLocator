using ClimateLocator.Core.Models;
using Newtonsoft.Json;

namespace ClimateLocator.Services
{
    public class WeatherbitService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public WeatherbitService(HttpClient httpClient, string apiKey)
        {
            _httpClient = httpClient;
            _apiKey = apiKey;
        }

        public async Task<WeatherData> GetWeatherDataByLocation(IpLocation location)
        {
            var response = await _httpClient.GetAsync($"https://api.weatherbit.io/v2.0/current?lat={location.Latitude}&lon={location.Longitude}={_apiKey}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<WeatherData>(content);
            }

            return null;
        }
    }
}
