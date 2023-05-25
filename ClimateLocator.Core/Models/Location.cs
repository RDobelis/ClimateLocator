using Newtonsoft.Json;

namespace ClimateLocator.Core.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
