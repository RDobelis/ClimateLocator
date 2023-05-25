using Newtonsoft.Json;

namespace ClimateLocator.Core.Models
{
    public class WeatherDescription : Entity
    {
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
