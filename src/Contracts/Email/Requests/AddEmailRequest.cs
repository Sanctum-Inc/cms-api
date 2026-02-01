namespace Contracts.Email.Requests;

public record AddEmailRequest
(
    // Recipients
    IReadOnlyCollection<string> To,
    IReadOnlyCollection<string>? Cc,
    IReadOnlyCollection<string>? Bcc,

    // Content
    string Subject,
    string Body,
    bool IsHtml,

    // Attachments / metadata
    IReadOnlyCollection<Guid>? AttachmentIds
);
