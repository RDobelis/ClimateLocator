using Newtonsoft.Json;

namespace ClimateLocator.Core.Models
{
    public class WeatherResponse
    {
        [JsonProperty("data")]
        public List<Weather> Data { get; set; }
    }
}
