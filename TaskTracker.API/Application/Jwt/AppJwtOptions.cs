namespace TaskTracker.API.Application.Jwt
{
    public class AppJwtOptions
    {
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public string SecretKey { get; set; } = null!;
        public int ExpiryDays { get; set; } = 7;
    }
}
