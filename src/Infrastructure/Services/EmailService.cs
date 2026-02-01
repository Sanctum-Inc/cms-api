using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Common.Interfaces.Session;
using Application.Common.Models;
using Application.Email.Commands.Add;
using Application.Email.Commands.Update;
using Domain.Emails;
using ErrorOr;
using Infrastructure.Services.Base;
using MapsterMapper;

namespace Infrastructure.Services;
public class EmailService : BaseService<Email, EmailResult, AddCommand, UpdateCommand>, IEmailService
{
    public EmailService(
        IEmailRepository _emailRepository,
        IMapper mapper,
        ISessionResolver sessionResolver) : base(_emailRepository, mapper, sessionResolver)
    {
    }

    protected override Guid GetIdFromUpdateCommand(UpdateCommand command)
    {
        return command.Id;
    }

    protected override ErrorOr<Email> MapFromAddCommand(AddCommand command, string? userId = null)
    {
        var email = new Email()
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
        };

        return email;
    }

    protected override void MapFromUpdateCommand(Email entity, UpdateCommand command)
    {
        entity.Status = command.Status;
    }
}
