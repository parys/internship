namespace Elevel.Domain.Models
{
    public class TokenConfiguration
    {
        public string Key { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public double DurationInMinutes { get; set; }
    }
}
