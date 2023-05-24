using ClimateLocator.Core.Models;

namespace ClimateLocator.Core.Interfaces
{
    public interface IWeatherService
    {
        Task<WeatherData> GetWeatherAsync(IpLocation location);
    }
}
