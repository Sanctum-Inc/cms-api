using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Common.Interfaces.Session;
using Application.Common.Models;
using Application.Email.Commands.Add;
using Application.Email.Commands.Update;
using Domain.Emails;
using ErrorOr;
using MapsterMapper;

namespace Infrastructure.Services;

public class EmailService : BaseService<Email, EmailResult, AddCommand, UpdateCommand>, IEmailService
{
    private readonly ISessionResolver _sessionResolver;

    public EmailService(
        IMapper mapper,
        IEmailRepository emailRepository,
        ISessionResolver sessionResolver) : base(emailRepository, mapper, sessionResolver)
    {
        _sessionResolver = sessionResolver;
    }

    protected override Guid GetIdFromUpdateCommand(UpdateCommand command)
    {
        return command.Id;
    }

    protected override ErrorOr<Email> MapFromAddCommand(AddCommand command, string? userId = null)
    {
        if (string.IsNullOrEmpty(_sessionResolver.UserId))
        {
            return Error.Unauthorized("User.NotFound", "User not found.");
        }

        var email = new Email
        {
            Id = Guid.NewGuid(),
            ToAddresses = string.Join(";", command.To),
            Body = command.Body,
            Subject = command.Subject,
            IsHtml = command.IsHtml,
            RetryCount = 0,
            Status = EmailStatus.Pending,
            AttachmentIds = command.AttachmentIds != null ? string.Join(";", command.AttachmentIds) : null,
            BccAddresses = command.Bcc != null ? string.Join(";", command.Bcc) : null,
            CcAddresses = command.Cc != null ? string.Join(";", command.Cc) : null,
            ErrorMessage = null,
            SentAt = null,
            UserId = new Guid(_sessionResolver.UserId)
        };

        return email;
    }

    protected override void MapFromUpdateCommand(Email entity, UpdateCommand command)
    {
        entity.Status = command.Status;
    }
}
