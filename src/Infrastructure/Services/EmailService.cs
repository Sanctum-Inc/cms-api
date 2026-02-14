using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Common.Interfaces.Session;
using Application.Common.Models;
using Application.Email.Commands.Add;
using Application.Email.Commands.Update;
using Domain.Emails;
using ErrorOr;
using Infrastructure.Config;
using MailKit.Net.Smtp;
using MapsterMapper;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Infrastructure.Services;

public class EmailService : BaseService<Email, EmailResult, AddCommand, UpdateCommand>, IEmailService
{
    private readonly ISessionResolver _sessionResolver;
    private readonly IEmailRepository _emailRepository;
    private readonly EmailOptions _emailOptions;

    public EmailService(
        IMapper mapper,
        IEmailRepository emailRepository,
        ISessionResolver sessionResolver,
        IOptions<EmailOptions> options) : base(emailRepository, mapper, sessionResolver)
    {
        _sessionResolver = sessionResolver;
        _emailRepository = emailRepository;
        _emailOptions = options.Value;
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

    public async Task<Email?> GetUnsentEmails(CancellationToken cancellationToken)
    {
        var email = await _emailRepository.GetUnsentEmails(cancellationToken);

        return email;
    }

    public async Task SendEmails(Email? email, CancellationToken cancellationToken)
    {
        if (email is null)
            return;

        using var client = new SmtpClient();
        try
        {
            email.Status = EmailStatus.Sending;
            await _emailRepository.SaveChangesAsync(cancellationToken);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailOptions.EmailName, _emailOptions.EmailAddress));
            message.To.Add(new MailboxAddress(email.ToAddresses, email.ToAddresses));
            if (email.BccAddresses is not null)
            {
                message.Bcc.Add(new MailboxAddress(email.BccAddresses, email.BccAddresses));
            }
            message.Subject = email.Subject;

            if (email.IsHtml)
            {
                message.Body = new TextPart("html")
                {
                    Text = email.Body
                };
            }
            else
            {
                message.Body = new TextPart("plain")
                {
                    Text = email.Body
                };
            }


            // Connect to your SMTP server (e.g., Gmail, Outlook, or a transactional service)
            await client.ConnectAsync(_emailOptions.Host, _emailOptions.Port, MailKit.Security.SecureSocketOptions.StartTls,
                cancellationToken);

            // Note: For services like Gmail, you might need to use an "App Password" instead of your actual password.
            await client.AuthenticateAsync(_emailOptions.Username, _emailOptions.Password, cancellationToken);

            await client.SendAsync(message, cancellationToken);

            email.SentAt = DateTime.UtcNow;
            email.Status = EmailStatus.Sent;
            await _emailRepository.SaveChangesAsync(cancellationToken);
            await client.DisconnectAsync(true, cancellationToken);
        }
        catch (Exception ex)
        {


            if (email.RetryCount >= 3)
            {
                email.ErrorMessage  = $"{ex.Message} {DateTime.UtcNow}";
                email.Status = EmailStatus.Failed;
                await _emailRepository.SaveChangesAsync(cancellationToken);
            }
            else
            {
                email.RetryCount++;
                email.Status = EmailStatus.Retrying;
                await _emailRepository.SaveChangesAsync(cancellationToken);
                await SendEmails(email, cancellationToken);
            }

        }
        finally
        {
            await client.DisconnectAsync(true, cancellationToken);
        }
    }
}
