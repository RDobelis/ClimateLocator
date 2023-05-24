using ClimateLocator.Core.Models;

namespace ClimateLocator.Core.Interfaces
{
    public interface ICacheService
    {
        Task<GeolocationInfo> GetGeolocationInfo(string ipAddress);
        Task<WeatherInfo> GetWeatherInfo(double latitude, double longitude);
        Task StoreGeolocationInfo(GeolocationInfo geolocationInfo);
        Task StoreWeatherInfo(WeatherInfo weatherInfo);
    }
}
