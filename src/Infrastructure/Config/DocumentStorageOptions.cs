namespace Infrastructure.Config;

public class DocumentStorageOptions
{
    public const string SectionName = "DocumentStorage";
    public string RootPath { get; set; } = string.Empty;
}
