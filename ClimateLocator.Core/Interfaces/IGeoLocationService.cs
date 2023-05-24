using ClimateLocator.Core.Models;

namespace ClimateLocator.Core.Interfaces
{
    public interface IGeoLocationService
    {
        Task<IpLocation> GetIpLocationAsync();
    }
}
