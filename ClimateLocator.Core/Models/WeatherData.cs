namespace ClimateLocator.Core.Models
{
    public class WeatherData
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Description { get; set; }
        public double Temperature { get; set; }
        public DateTime QueriedAt { get; set; }
    }
}
