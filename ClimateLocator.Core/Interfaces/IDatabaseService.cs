using ClimateLocator.Core.Models;

namespace ClimateLocator.Core.Interfaces
{
    public interface IDatabaseService
    {
        Task StoreGeolocationInfo(GeolocationInfo geolocationInfo);
        Task StoreWeatherInfo(WeatherInfo weatherInfo);
        Task<HistoricalGeolocationInfo> GetHistoricalGeolocationInfo(string ipAddress);
        Task<HistoricalWeatherInfo> GetHistoricalWeatherInfo(double latitude, double longitude);
    }
}
