using ClimateLocator.Core.Models;

namespace ClimateLocator.Core.Interfaces
{
    public interface IDataStorageService
    {
        Task StoreIpLocationAsync(IpLocation location);
        Task StoreWeatherDataAsync(WeatherData weatherData);
        Task<IpLocation> GetIpLocationAsync(string ip);
        Task<WeatherData> GetWeatherDataAsync(IpLocation location);
    }
}
