using Newtonsoft.Json;

namespace ClimateLocator.Core.Models
{
    public class WeatherDescription
    {
        public int Id { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
