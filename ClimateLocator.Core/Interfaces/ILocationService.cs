using ClimateLocator.Core.Models;

namespace ClimateLocator.Core.Interfaces
{
    public interface ILocationService
    {
        Task<Location> GetLocationAsync(string ip);
    }
}
