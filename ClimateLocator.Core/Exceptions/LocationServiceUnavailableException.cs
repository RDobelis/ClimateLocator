namespace ClimateLocator.Core.Exceptions
{
    public class LocationServiceUnavailableException : Exception
    {
        public LocationServiceUnavailableException() : base("Location service is unavailable.")
        {
        }
    }
}