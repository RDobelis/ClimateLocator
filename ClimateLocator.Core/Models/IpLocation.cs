namespace ClimateLocator.Core.Models
{
    public class IpLocation
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime QueriedAt { get; set; }
    }
}
