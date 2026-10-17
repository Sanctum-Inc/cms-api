namespace Application.Common.Models;

public class EmailMessage
{
    public required string FromName { get; set; }
    public required string FromAddress { get; set; }
    public required List<string> To { get; set; }
    public List<string> Cc { get; set; } = [];
    public List<string> Bcc { get; set; } = [];
    public string? ReplyToName { get; set; }
    public string? ReplyToAddress { get; set; }
    public required string Subject { get; set; }
    public required string Body { get; set; }
    public required bool IsHtml { get; set; }
}
