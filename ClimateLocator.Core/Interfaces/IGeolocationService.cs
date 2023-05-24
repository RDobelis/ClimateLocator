using ClimateLocator.Core.Models;

namespace ClimateLocator.Core.Interfaces
{
    public interface IGeolocationService
    {
        Task<GeolocationInfo> GetGeolocationInfo(string ipAddress);
    }
}
