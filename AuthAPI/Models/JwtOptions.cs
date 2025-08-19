namespace OrderBooking.Services.AuthAPI.Models
{
    public class JwtOptions
    {
        public string JwtSecret { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
    }
}
