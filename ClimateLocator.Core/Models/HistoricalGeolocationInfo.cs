namespace ClimateLocator.Core.Models
{
    public class HistoricalGeolocationInfo
    {
        public int Id { get; set; }
        public string IPAddress { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}

