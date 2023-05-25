using ClimateLocator.Core.Interfaces;
using ClimateLocator.Core.Models;
using ClimateLocator.Data;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework.Internal;

namespace ClimateLocator.Tests
{
    public class DataProviderTests
    {
        private Mock<ILocationService> _locationService;
        private Mock<IWeatherService> _weatherService;
        private Mock<IMemoryCache> _cache;
        private DataProvider _dataProvider;
        private static string _testIp = "52.60.48.246";

        private static Weather _fakeWeather = new Weather
        {
            Temperature = 20,
            WeatherDescription = new WeatherDescription()
            {
                Description = "Sunny"
            }
        };

        [SetUp]
        public void Setup()
        {
            _locationService = new Mock<ILocationService>();
            _weatherService = new Mock<IWeatherService>();
            _cache = new Mock<IMemoryCache>();
            _dataProvider = new DataProvider(
                _locationService.Object,
                _weatherService.Object,
                _cache.Object);
        }

        [Test]
        public async Task GetWeatherAsync_CallsServices_WhenDataIsNotCached()
        {
            object cacheValue;
            _cache.Setup(x => x.TryGetValue(It.IsAny<object>(), out cacheValue)).Returns(false);
            _locationService.Setup(x => x.GetLocationAsync(_testIp)).ReturnsAsync(new Location());
            _weatherService.Setup(x => x.GetWeatherAsync(It.IsAny<Location>())).ReturnsAsync(_fakeWeather);
            _cache.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>());

            var weather = await _dataProvider.GetWeatherAsync(_testIp);

            _locationService.Verify(x => x.GetLocationAsync(_testIp), Times.Once);
            _weatherService.Verify(x => x.GetWeatherAsync(It.IsAny<Location>()), Times.Once);
            weather.Should().BeEquivalentTo(_fakeWeather);
        }

        [Test]
        public async Task GetWeatherAsync_UsesCache_WhenDataIsCached()
        {
            object cacheValue = _fakeWeather;
            _cache.Setup(x => x.TryGetValue(It.IsAny<object>(), out cacheValue)).Returns(true);

            var weather = await _dataProvider.GetWeatherAsync(_testIp);

            _locationService.Verify(x => x.GetLocationAsync(_testIp), Times.Never);
            _weatherService.Verify(x => x.GetWeatherAsync(It.IsAny<Location>()), Times.Never);
            weather.Should().BeEquivalentTo(_fakeWeather);
        }

    }
}
