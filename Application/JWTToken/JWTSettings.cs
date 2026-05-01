namespace Application.JWTToken;

public class JwtSettings
{
    public const string Section = "Jwt";

    public string SecretKey { get; set; } = null!; 
    public string Issuer { get; set; } = null!; 
    public string Audience { get; set; } = null!; 
    public int ExpiryMinutes { get; set; } = 1440;
}
