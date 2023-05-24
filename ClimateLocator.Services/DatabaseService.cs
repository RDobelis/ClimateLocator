using ClimateLocator.Core.Interfaces;
using ClimateLocator.Core.Models;
using ClimateLocator.Data;
using Microsoft.EntityFrameworkCore;

namespace ClimateLocator.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly ClimateLocatorDbContext _context;

        public DatabaseService(ClimateLocatorDbContext context)
        {
            _context = context;
        }

        public async Task StoreGeolocationInfo(GeolocationInfo geolocationInfo)
        {
            var historicalInfo = new HistoricalGeolocationInfo
            {
                IPAddress = geolocationInfo.IPAddress,
                City = geolocationInfo.City,
                Country = geolocationInfo.Country,
                Latitude = geolocationInfo.Latitude,
                Longitude = geolocationInfo.Longitude
            };

            _context.HistoricalGeolocations.Add(historicalInfo);
            await _context.SaveChangesAsync();
        }

        public async Task StoreWeatherInfo(WeatherInfo weatherInfo)
        {
            var historicalInfo = new HistoricalWeatherInfo
            {
                Latitude = weatherInfo.Latitude,
                Longitude = weatherInfo.Longitude,
                Temperature = weatherInfo.Temperature,
                Humidity = weatherInfo.Humidity,
                Description = weatherInfo.Description,
                Timestamp = weatherInfo.Timestamp
            };

            _context.HistoricalWeathers.Add(historicalInfo);
            await _context.SaveChangesAsync();
        }

        public async Task<HistoricalGeolocationInfo> GetHistoricalGeolocationInfo(string ipAddress)
        {
            return await _context.HistoricalGeolocations
                .FirstOrDefaultAsync(x => x.IPAddress == ipAddress);
        }

        public async Task<HistoricalWeatherInfo> GetHistoricalWeatherInfo(double latitude, double longitude)
        {
            return await _context.HistoricalWeathers
                .FirstOrDefaultAsync(x => x.Latitude == latitude && x.Longitude == longitude);
        }
    }
}
