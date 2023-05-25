namespace ClimateLocator.Core.Models
{
    public class Query : Entity
    {
        public string Ip { get; set; }
        public DateTime QuerriedAt { get; set; } = DateTime.UtcNow;
    }
}
