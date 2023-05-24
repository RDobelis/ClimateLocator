namespace ClimateLocator.Core.Services
{
    public class WeatherResponse
    {
        public double Temp { get; set; }
        public int Rh { get; set; }
        public WeatherDetails Weather { get; set; }
    }
}
