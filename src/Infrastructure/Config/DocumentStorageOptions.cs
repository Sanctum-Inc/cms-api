namespace Infrastructure.Config;

public class DocumentStorageOptions
{
    public const string SectionName = "DocumentStorage";
    public string Provider { get; set; } = DocumentStorageProviders.Local;
    public string RootPath { get; set; } = string.Empty;
    public S3StorageOptions S3 { get; set; } = new();
}

public class S3StorageOptions
{
    public string Endpoint { get; set; } = string.Empty;
    public string AccessKeyId { get; set; } = string.Empty;
    public string SecretAccessKey { get; set; } = string.Empty;
    public string BucketName { get; set; } = string.Empty;
}

public static class DocumentStorageProviders
{
    public const string Local = "Local";
    public const string S3 = "S3";
}
