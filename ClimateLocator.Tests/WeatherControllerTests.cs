using ClimateLocator.API.Controllers;
using ClimateLocator.Core.Models;
using ClimateLocator.Data;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ClimateLocator.Tests
{
    public class WeatherControllerTests
    {
        private Mock<IDataProvider> _dataProvider;
        private WeatherController _weatherController;
        private static string _testIp = "52.60.48.246";

        [SetUp]
        public void Setup()
        {
            _dataProvider = new Mock<IDataProvider>();
            _weatherController = new WeatherController(_dataProvider.Object);
        }

        [Test]
        public async Task GetWeather_ReturnBadRequest_WhenIpIsNull()
        {
            var result = await _weatherController.GetWeather(null);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task GetWeather_ReturnNotFound_WhenWeatherIsNull()
        {
            _dataProvider.Setup(x => x.GetWeatherAsync(_testIp)).ReturnsAsync((Weather)null);

            var result = await _weatherController.GetWeather(_testIp);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Test]
        public async Task GetWeather_ReturnWeather_WhenWeatherIsNotNull()
        {
            _dataProvider.Setup(x => x.GetWeatherAsync(_testIp)).ReturnsAsync(new Weather());

            var result = await _weatherController.GetWeather(_testIp);

            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
