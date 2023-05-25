namespace ClimateLocator.Core.Exceptions
{
    public class WeatherServiceUnavailableException : Exception
    {
        public WeatherServiceUnavailableException() : base("Weather service unavailable.")
        {
        }
    }
}

