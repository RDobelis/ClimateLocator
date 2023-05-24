using ClimateLocator.Core.Interfaces;
using ClimateLocator.Core.Models;
using ClimateLocator.Core.Services;
using Newtonsoft.Json;

public class WeatherService : IWeatherService
{
    private readonly ICacheService _cacheService;
    private readonly IDatabaseService _databaseService;
    private readonly HttpClient _client;

    public WeatherService(ICacheService cacheService, IDatabaseService databaseService, HttpClient client)
    {
        _cacheService = cacheService;
        _databaseService = databaseService;
        _client = client;
    }

    public async Task<WeatherInfo> GetWeatherInfo(double latitude, double longitude)
    {
        // Get data from the cache
        var cachedInfo = await _cacheService.GetWeatherInfo(latitude, longitude);
        if (cachedInfo != null)
            return cachedInfo;

        // Get data from the database
        var historicalInfo = await _databaseService.GetHistoricalWeatherInfo(latitude, longitude);
        if (historicalInfo != null)
            return new WeatherInfo
            {
                Latitude = historicalInfo.Latitude,
                Longitude = historicalInfo.Longitude,
                Temperature = historicalInfo.Temperature,
                Humidity = historicalInfo.Humidity,
                Description = historicalInfo.Description,
                Timestamp = historicalInfo.Timestamp
            };

        // Fetch data from the external API
        var response = await _client.GetStringAsync($"?lat={latitude}&lon={longitude}");
        var data = JsonConvert.DeserializeObject<WeatherResponse>(response);
        var weatherInfo = new WeatherInfo
        {
            Latitude = latitude,
            Longitude = longitude,
            Temperature = data.Temp,
            Humidity = data.Rh,
            Description = Enum.Parse<WeatherDescription>(data.Weather.Description),

            Timestamp = DateTime.Now
        };

        // Store data in the cache and database
        await _cacheService.StoreWeatherInfo(weatherInfo);
        await _databaseService.StoreWeatherInfo(weatherInfo);

        return weatherInfo;
    }
}