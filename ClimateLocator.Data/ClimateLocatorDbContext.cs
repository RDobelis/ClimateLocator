using ClimateLocator.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ClimateLocator.Data
{
    public class ClimateLocatorDbContext : DbContext
    {
        public ClimateLocatorDbContext(DbContextOptions<ClimateLocatorDbContext> options) : base(options)
        {
        }

        public DbSet<Weather> Weather { get; set; }
        public DbSet<Location> Location { get; set; }
    }
}
