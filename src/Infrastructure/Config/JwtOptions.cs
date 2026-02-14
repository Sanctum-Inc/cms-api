namespace Infrastructure.Config;

public class JwtOptions
{
    public const string SectionName = "JwtOptions";
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string ExpireMinutes { get; set; } = string.Empty;

}
