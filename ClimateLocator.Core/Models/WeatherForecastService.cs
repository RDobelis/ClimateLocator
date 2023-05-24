using ClimateLocator.Core.Interfaces;

namespace ClimateLocator.Core.Models
{
    public class WeatherForecastService : IWeatherForecastService
    {
        private readonly IGeolocationService _geolocationService;
        private readonly IWeatherService _weatherService;

        public WeatherForecastService(IGeolocationService geolocationService, IWeatherService weatherService)
        {
            _geolocationService = geolocationService;
            _weatherService = weatherService;
        }

        public async Task<WeatherInfo> GetWeatherForecast(string ipAddress)
        {
            var geolocationInfo = await _geolocationService.GetGeolocationInfo(ipAddress);

            if (geolocationInfo == null) 
                return null;

            var weatherInfo = await _weatherService.GetWeatherInfo(geolocationInfo.Latitude, geolocationInfo.Longitude);
            return weatherInfo;
        }
    }
}
