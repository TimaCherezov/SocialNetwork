namespace Application.Settings;

public class JwtSettings
{
    public const string Section = "Jwt";
    public string SecretKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpiryMinutes { get; set; }
}

