using ClimateLocator.Services;
using Microsoft.Extensions.Configuration;
using FluentAssertions;

namespace ClimateLocator.Tests
{
    public class LocationServiceTests
    {
        private LocationService _locationService;
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
            _locationService = new LocationService(_httpClient, _configuration);
        }

        [Test]
        public async Task GetLocationAsync_WithValidIp_ReturnsLocation()
        {
            var location = await _locationService.GetLocationAsync(_testIp);

            location.Should().NotBeNull();
            location.Ip.Should().Be(_testIp);
        }
    }
}
 