using Domain.Emails;

namespace Contracts.Email.Response;
public record EmailResponse
(
    // Recipients
    string ToAddresses,
    string? CcAddresses,
    string? BccAddresses,

    // Content
    string Subject,
    string Body,

    // Attachments / metadata
    string? AttachmentIds,

    // Status
    EmailStatus Status,
    string? ErrorMessage,

    // Audit
    DateTime? SentAt
);
