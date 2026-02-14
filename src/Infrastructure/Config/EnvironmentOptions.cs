namespace Infrastructure.Config;

public class EnvironmentOptions
{
    public const string SectionName = "EnvironmentOptions";

    public string FrontendUrl { get; set; } = string.Empty;
    public string BackendUrl { get; set; } = string.Empty;
}
