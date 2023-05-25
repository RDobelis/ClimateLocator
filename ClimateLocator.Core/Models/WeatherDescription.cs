using Newtonsoft.Json;

namespace ClimateLocator.Core.Models
{
    public class WeatherDescription
    {
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
