namespace Infrastructure.Config;

public class PdfOptions
{
    public const string SectionName = "PdfOptions";

    /// <summary>Time in minutes before the signed URL expires</summary>
    public int LinkExpiryMinutes { get; set; } = 1;

    /// <summary>Optional: override host for generated links (useful in dev/staging)</summary>
    public string? BaseUrl { get; set; }

    /// <summary>Secret key for signing URLs</summary>
    public string SigningKey { get; set; } = string.Empty;
}
