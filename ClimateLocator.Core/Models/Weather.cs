using Newtonsoft.Json;

namespace ClimateLocator.Core.Models
{
    public class Weather : Entity
    {
        [JsonProperty("app_temp")]
        public double Temperature { get; set; }
        [JsonProperty("weather")]
        public WeatherDescription WeatherDescription { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int LocationId { get; set; }
        public Location Location { get; set; }
    }
}
