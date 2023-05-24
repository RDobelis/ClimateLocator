using ClimateLocator.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ClimateLocator.Data
{
    public class ClimateLocatorDbContext : DbContext
    {
        public ClimateLocatorDbContext(DbContextOptions<ClimateLocatorDbContext> options)
            : base(options)
        {
        }

        public DbSet<HistoricalGeolocationInfo> HistoricalGeolocations { get; set; }
        public DbSet<HistoricalWeatherInfo> HistoricalWeathers { get; set; }
    }
}