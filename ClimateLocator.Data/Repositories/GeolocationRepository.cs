using ClimateLocator.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ClimateLocator.Data.Repositories
{
    public class GeolocationRepository
    {
        private readonly ClimateLocatorDbContext _context;

        public GeolocationRepository(ClimateLocatorDbContext context)
        {
            _context = context;
        }

        public async Task<HistoricalGeolocationInfo> GetInfo(string ipAddress)
        {
            return await _context.HistoricalGeolocations
                .FirstOrDefaultAsync(x => x.IPAddress == ipAddress);
        }

        public async Task AddInfo(HistoricalGeolocationInfo info)
        {
            _context.HistoricalGeolocations.Add(info);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateInfo(HistoricalGeolocationInfo info)
        {
            _context.HistoricalGeolocations.Update(info);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteInfo(HistoricalGeolocationInfo info)
        {
            _context.HistoricalGeolocations.Remove(info);
            await _context.SaveChangesAsync();
        }
    }
}
