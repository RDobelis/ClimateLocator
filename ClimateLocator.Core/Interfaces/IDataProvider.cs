using ClimateLocator.Core.Models;

namespace ClimateLocator.Data;

public interface IDataProvider
{
    Task<Weather> GetWeatherAsync(string ip);
}