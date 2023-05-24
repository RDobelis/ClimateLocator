using ClimateLocator.Core.Interfaces;
using ClimateLocator.Core.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using ClimateLocator.Core.Services;

public class GeolocationService : IGeolocationService
{
    private readonly ICacheService _cacheService;
    private readonly IDatabaseService _databaseService;
    private readonly HttpClient _client;

    public GeolocationService(ICacheService cacheService, IDatabaseService databaseService, HttpClient client)
    {
        _cacheService = cacheService;
        _databaseService = databaseService;
        _client = client;
    }

    public async Task<GeolocationInfo> GetGeolocationInfo(string ipAddress)
    {
        // Get data from the cache
        var cachedInfo = await _cacheService.GetGeolocationInfo(ipAddress);
        if (cachedInfo != null)
            return cachedInfo;

        // Get data from the database
        var historicalInfo = await _databaseService.GetHistoricalGeolocationInfo(ipAddress);
        if (historicalInfo != null)
            return new GeolocationInfo
            {
                IPAddress = historicalInfo.IPAddress,
                City = historicalInfo.City,
                Country = historicalInfo.Country,
                Latitude = historicalInfo.Latitude,
                Longitude = historicalInfo.Longitude
            };

        // Fetch data from the external API
        var response = await _client.GetStringAsync($"?ip={ipAddress}");
        var data = JsonConvert.DeserializeObject<GeolocationResponse>(response);
        var geolocationInfo = new GeolocationInfo
        {
            IPAddress = ipAddress,
            City = data.CityName,
            Country = data.CountryName,
            Latitude = data.Latitude,
            Longitude = data.Longitude
        };

        // Store data in the cache and database
        await _cacheService.StoreGeolocationInfo(geolocationInfo);
        await _databaseService.StoreGeolocationInfo(geolocationInfo);

        return geolocationInfo;
    }
}