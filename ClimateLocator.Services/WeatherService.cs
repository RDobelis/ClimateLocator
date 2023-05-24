using AutoMapper;
using ClimateLocator.Core.Interfaces;
using ClimateLocator.Core.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Net.Http;

public class WeatherService : IWeatherService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IDataStorageService _dataStorageService;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache;
    private readonly string _geolocationApiKey;
    private readonly string _weatherApiKey;

    public WeatherService(IHttpClientFactory httpClientFactory, IDataStorageService dataStorageService, IMapper mapper, IMemoryCache cache, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _dataStorageService = dataStorageService;
        _mapper = mapper;
        _cache = cache;
        _geolocationApiKey = configuration["GeolocationApiKey"];
        _weatherApiKey = configuration["WeatherApiKey"];
    }

    public async Task<WeatherData> GetWeatherAsync(IpLocation ip)
    {
        var httpClient = _httpClientFactory.CreateClient();
        IpLocation ipLocation;
        if (!_cache.TryGetValue(ip, out ipLocation))
        {
            var ipLocationResponse = await httpClient.GetAsync($"https://api.ipgeolocation.io/ipgeo?apiKey={_geolocationApiKey}&ip={ip}");
            ipLocationResponse.EnsureSuccessStatusCode();

            var ipLocationResponseContent = await ipLocationResponse.Content.ReadAsStringAsync();
            var ipLocationDto = JsonConvert.DeserializeObject<IpLocationDto>(ipLocationResponseContent);

            ipLocation = _mapper.Map<IpLocation>(ipLocationDto);
            ipLocation.QueriedAt = DateTime.UtcNow;

            await _dataStorageService.StoreIpLocationAsync(ipLocation);

            // Set cache options
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            // Save data in cache.
            _cache.Set(ip, ipLocation, cacheEntryOptions);
        }

        var weatherData = await _cache.GetOrCreateAsync<WeatherData>($"{ip}_weather", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            var weatherResponse = await httpClient.GetAsync($"https://api.openweathermap.org/data/2.5/weather?lat={ipLocation.Latitude}&lon={ipLocation.Longitude}&appid={_weatherApiKey}");
            weatherResponse.EnsureSuccessStatusCode();

            var weatherResponseContent = await weatherResponse.Content.ReadAsStringAsync();
            var weatherDto = JsonConvert.DeserializeObject<WeatherDataDto>(weatherResponseContent);

            var weatherData = _mapper.Map<WeatherData>(weatherDto);
            weatherData.QueriedAt = DateTime.UtcNow;

            await _dataStorageService.StoreWeatherDataAsync(weatherData);

            return weatherData;
        });

        return weatherData;
    }
}
