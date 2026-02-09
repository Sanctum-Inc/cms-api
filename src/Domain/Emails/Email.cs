using Domain.Common;
using Domain.Users;

namespace Domain.Emails;

public class Email : AuditableEntity
{
    // Recipients
    public required string ToAddresses { get; set; }
    public string? CcAddresses { get; set; }
    public string? BccAddresses { get; set; }

    // Content
    public required string Subject { get; set; } = default!;
    public required string Body { get; set; } = default!;
    public required bool IsHtml { get; set; }

    // Attachments / metadata
    public string? AttachmentIds { get; set; }

    // Status
    public required EmailStatus Status { get; set; }
    public required int RetryCount { get; set; }
    public string? ErrorMessage { get; set; }

    // Audit
    public DateTime? SentAt { get; set; }

    // 🔗 User linkage
    public required Guid UserId { get; set; }
    public User? User { get; set; }
}
