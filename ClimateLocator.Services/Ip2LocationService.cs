using Castle.Components.DictionaryAdapter.Xml;
using Castle.Core.Configuration;
using ClimateLocator.Core.Models;
using Newtonsoft.Json;

namespace ClimateLocator.Services
{
    public class Ip2LocationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public Ip2LocationService(HttpClient httpClient, string apiKey)
        {
            _httpClient = httpClient;
            _apiKey = apiKey;
        }

        public async Task<IpLocation> GetLocationByIp(string ip)
        {
            var response = await _httpClient.GetAsync($"https://api.ip2location.io/?key={_apiKey}&ip={ip}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IpLocation>(content);
            }

            return null;
        }
    }
}
