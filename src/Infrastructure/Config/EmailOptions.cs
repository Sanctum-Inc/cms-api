using System.Text;

namespace Infrastructure.Config;

public class EmailOptions
{
    public const string SectionName = "EmailOptions";

    public string Provider { get; set; } = EmailProviders.Smtp;
    public bool Enabled { get; set; }
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 0;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string EmailName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public int PollIntervalSeconds { get; set; } = 1;
}

public static class EmailProviders
{
    public const string Smtp = "Smtp";
    public const string Brevo = "Brevo";
}
