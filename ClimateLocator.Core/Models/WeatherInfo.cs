namespace ClimateLocator.Core.Models
{
    public class WeatherInfo
    {
        public double Temp { get; set; }
        public double Rh { get; set; }
        public WeatherDescription Weather { get; set; }
    }
}