using ClimateLocator.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ClimateLocator.Data.Repositories
{
    public class WeatherRepository
    {
        private readonly ClimateLocatorDbContext _context;

        public WeatherRepository(ClimateLocatorDbContext context)
        {
            _context = context;
        }

        public async Task<HistoricalWeatherInfo> GetInfo(double latitude, double longitude)
        {
            return await _context.HistoricalWeathers
                .FirstOrDefaultAsync(x => x.Latitude == latitude && x.Longitude == longitude);
        }

        public async Task AddInfo(HistoricalWeatherInfo info)
        {
            _context.HistoricalWeathers.Add(info);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateInfo(HistoricalWeatherInfo info)
        {
            _context.HistoricalWeathers.Update(info);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteInfo(HistoricalWeatherInfo info)
        {
            _context.HistoricalWeathers.Remove(info);
            await _context.SaveChangesAsync();
        }
    }
}
