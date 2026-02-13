using System.Text;

namespace Infrastructure.Config;

public class EmailOptions
{
    public const string SectionName = "EmailOptions";
    public string Host { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string EmailName { get; set; }
    public string EmailAddress { get; set; }
}
