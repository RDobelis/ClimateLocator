using ClimateLocator.Services;
using Microsoft.Extensions.Configuration;
using FluentAssertions;

namespace ClimateLocator.Tests
{
    public class WeatherServiceTests
    {
        private WeatherService _weatherService;
        private HttpClient _httpClient;
        private IConfiguration _configuration;
        private static string _testIp = "52.60.48.246";

        [SetUp]
        public void Setup()
        {
            _httpClient = new HttpClient();
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            _weatherService = new WeatherService(_httpClient, _configuration);
        }

        [Test]
        public async Task GetWeatherAsync_WithValidIp_ReturnsWeather()
        {
            var locationService = new LocationService(_httpClient, _configuration);

            var location = await locationService.GetLocationAsync(_testIp);
            var weather = await _weatherService.GetWeatherAsync(location);

            weather.Should().NotBeNull();
            weather.Temperature.Should().NotBe(null);
            weather.WeatherDescription.Should().NotBeNull();
        }
    }
}
