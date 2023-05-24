using ClimateLocator.Core.Interfaces;
using ClimateLocator.Core.Models;
using ClimateLocator.Data;
using Microsoft.EntityFrameworkCore;

namespace ClimateLocator.Services
{
    public class DataStorageService : IDataStorageService
    {
        private readonly ClimateLocatorDbContext _context;

        public DataStorageService(ClimateLocatorDbContext context)
        {
            _context = context;
        }

        public async Task StoreIpLocationAsync(IpLocation location)
        {
            var existingLocation = await _context.IpLocations.SingleOrDefaultAsync(x => x.Ip == location.Ip);

            if (existingLocation == null)
            {
                _context.IpLocations.Add(location);
                await _context.SaveChangesAsync();
            }
            else
            {
                _context.IpLocations.Add(location);
            }

            await _context.SaveChangesAsync();
        }

        public async Task StoreWeatherDataAsync(WeatherData weatherData)
        {
            var existingWeatherData = await _context.WeatherData
                .FirstOrDefaultAsync(wd => wd.Latitude == weatherData.Latitude && wd.Longitude == weatherData.Longitude);

            if (existingWeatherData != null)
            {
                _context.Entry(existingWeatherData).CurrentValues.SetValues(weatherData);
            }
            else
            {
                _context.WeatherData.Add(weatherData);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IpLocation> GetIpLocationAsync(string ip)
        {
            return await _context.IpLocations.SingleOrDefaultAsync(x => x.Ip == ip);
        }

        public async Task<WeatherData> GetWeatherDataAsync(IpLocation location)
        {
            return await _context.WeatherData
                .FirstOrDefaultAsync(wd => wd.Latitude == location.Latitude && wd.Longitude == location.Longitude);
        }
    }
}
