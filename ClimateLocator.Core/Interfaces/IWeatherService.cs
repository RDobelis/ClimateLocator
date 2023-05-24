using ClimateLocator.Core.Models;

namespace ClimateLocator.Core.Interfaces
{
    public interface IWeatherService
    {
        Task<WeatherInfo> GetWeatherInfo(double latitude, double longitude);
    }
}
