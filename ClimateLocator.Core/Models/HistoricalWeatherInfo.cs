namespace ClimateLocator.Core.Models
{
    public class HistoricalWeatherInfo
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Temperature { get; set; }
        public string Humidity { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
    }
}