using System.Net;
using ClimateLocator.Core.Interfaces;
using ClimateLocator.Core.Models;
using ClimateLocator.Data;
using ClimateLocator.Services;
using Microsoft.Extensions.Configuration;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;


namespace ClimateLocator.Tests
{
    public class WeatherServiceTests
    {
        private WeatherService _weatherService;
        private Mock<HttpMessageHandler> _httpMessageHandler;
        private IConfiguration _configuration;
        private ClimateLocatorDbContext _context;

        [SetUp]
        public void Setup()
        {
            _httpMessageHandler = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(_httpMessageHandler.Object);
            var options = new DbContextOptionsBuilder<ClimateLocatorDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;
            _context = new ClimateLocatorDbContext(options);
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            _weatherService = new WeatherService(httpClient, _configuration, _context);
        }

        [Test]
        public async Task GetWeatherAsync_WithValidLocation_ReturnsWeather()
        {
            var location = new Location
            {
                Latitude = 123.456,
                Longitude = 789.012
            };

            var weatherResponse = new WeatherResponse
            {
                Data = new List<Weather>
                {
                    new Weather
                    {
                        Temperature = 123.456,
                        WeatherDescription = new WeatherDescription { Description = "Test Description" }
                    }
                }
            };

            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(weatherResponse))
            };

            _httpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(httpResponse);

            var weather = await _weatherService.GetWeatherAsync(location);


            weather.Should().NotBeNull();
            weather.Temperature.Should().NotBe(null);
            weather.WeatherDescription.Should().NotBeNull();
            var expectedUrl = $"https://api.weatherbit.io/v2.0/current?lat={location.Latitude}&lon={location.Longitude}&key={_configuration["WeatherApiKey"]}";
            _httpMessageHandler
                .Protected()
                .Verify<Task<HttpResponseMessage>>("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>
                    (req => req.RequestUri.ToString() == expectedUrl),
                    ItExpr.IsAny<System.Threading.CancellationToken>());
        }
    }
}
