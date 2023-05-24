using ClimateLocator.Core.Models;

namespace ClimateLocator.Core.Interfaces
{
    public interface IWeatherForecastService
    {
        Task<WeatherInfo> GetWeatherForecast(string ipAddress);
    }
}
