namespace ClimateLocator.Core.Services
{
    public class GeolocationResponse
    {
        public string CountryName { get; set; }
        public string CityName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
