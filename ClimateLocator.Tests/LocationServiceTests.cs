using ClimateLocator.Data;
using ClimateLocator.Services;
using Microsoft.Extensions.Configuration;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Polly.CircuitBreaker;
using Polly;
using System.Net;
using ClimateLocator.Core.Exceptions;
using ClimateLocator.Core.Models;
using Moq.Protected;
using Newtonsoft.Json;

namespace ClimateLocator.Tests
{
    public class LocationServiceTests
    {
        private LocationService _locationService;
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private IConfiguration _configuration;
        private ClimateLocatorDbContext _context;
        private static string _testIp = "52.60.48.246";

        [SetUp]
        public void Setup()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            var options = new DbContextOptionsBuilder<ClimateLocatorDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;
            _context = new ClimateLocatorDbContext(options);
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            _locationService = new LocationService(httpClient, _configuration, _context);
        }

        [Test]
        public async Task GetLocationAsync_WithValidIp_ReturnsLocation()
        {
            var locationResponse = new Location
            {
                Ip = _testIp
            };

            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(locationResponse))
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(httpResponse);

            var location = await _locationService.GetLocationAsync(_testIp);

            location.Should().NotBeNull();
            location.Ip.Should().Be(_testIp);
        } 
    }
}
 