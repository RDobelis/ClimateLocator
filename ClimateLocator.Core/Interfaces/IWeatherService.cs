using ClimateLocator.Core.Models;

namespace ClimateLocator.Core.Interfaces
{
    public interface IWeatherService
    {
        Task<Weather> GetWeatherAsync(Location location);
    }
}
