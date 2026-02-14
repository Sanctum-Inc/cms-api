using System.Text;

namespace Infrastructure.Config;

public class EmailOptions
{
    public const string SectionName = "EmailOptions";
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 0;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string EmailName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
}
