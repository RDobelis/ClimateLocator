namespace ClimateLocator.Core.Models
{
    public class WeatherInfo
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? Temperature { get; set; }
        public double? Humidity { get; set; }
        public WeatherDescription Description { get; set; }
        public DateTime Timestamp { get; set; }
    }
}