using ErrorOr;
using MediatR;

namespace Application.Email.Commands.Add;

public record AddCommand(
    // Recipients
    IReadOnlyCollection<string> To,
    IReadOnlyCollection<string>? Cc,
    IReadOnlyCollection<string>? Bcc,

    // Content
    string Subject,
    string Body,
    bool IsHtml,

    // Attachments / metadata
    IReadOnlyCollection<Guid>? AttachmentIds) : IRequest<ErrorOr<Guid>>;
