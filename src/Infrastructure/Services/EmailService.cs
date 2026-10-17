using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Common.Interfaces.Session;
using Application.Common.Models;
using Application.Email.Commands.Add;
using Application.Email.Commands.Update;
using Domain.Emails;
using ErrorOr;
using Infrastructure.Config;
using MapsterMapper;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class EmailService : BaseService<Email, EmailResult, AddCommand, UpdateCommand>, IEmailService
{
    private const int MaxRetries = 3;

    private readonly ISessionResolver _sessionResolver;
    private readonly IEmailRepository _emailRepository;
    private readonly EmailOptions _emailOptions;
    private readonly IEmailSender _emailSender;

    public EmailService(
        IMapper mapper,
        IEmailRepository emailRepository,
        ISessionResolver sessionResolver,
        IOptions<EmailOptions> options,
        IEmailSender emailSender) : base(emailRepository, mapper, sessionResolver)
    {
        _sessionResolver = sessionResolver;
        _emailRepository = emailRepository;
        _emailOptions = options.Value;
        _emailSender = emailSender;
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

        if (email.User is null)
        {
            email.ErrorMessage = "No user linked to this email.";
            email.Status = EmailStatus.Failed;
            await _emailRepository.SaveChangesAsync(cancellationToken);
            return;
        }

        var message = new EmailMessage
        {
            FromName = $"{email.User.Name} {email.User.Surname} via LexCase",
            FromAddress = _emailOptions.EmailAddress,
            To = SplitAddresses(email.ToAddresses),
            Cc = SplitAddresses(email.CcAddresses),
            Bcc = SplitAddresses(email.BccAddresses),
            ReplyToName = $"{email.User.Name} {email.User.Surname}",
            ReplyToAddress = email.User.Email,
            Subject = email.Subject,
            Body = email.Body,
            IsHtml = email.IsHtml
        };

        email.Status = EmailStatus.Sending;
        await _emailRepository.SaveChangesAsync(cancellationToken);

        for (var attempt = 0; attempt <= MaxRetries; attempt++)
        {
            try
            {
                await _emailSender.SendAsync(message, cancellationToken);

                email.SentAt = DateTime.UtcNow;
                email.Status = EmailStatus.Sent;
                await _emailRepository.SaveChangesAsync(cancellationToken);
                return;
            }
            catch (Exception ex) when (attempt < MaxRetries)
            {
                email.RetryCount = attempt + 1;
                email.Status = EmailStatus.Retrying;
                email.ErrorMessage = ex.Message;
                await _emailRepository.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                email.ErrorMessage = $"{ex.Message} {DateTime.UtcNow}";
                email.Status = EmailStatus.Failed;
                await _emailRepository.SaveChangesAsync(cancellationToken);
                return;
            }
        }
    }

    private static List<string> SplitAddresses(string? addresses)
    {
        return string.IsNullOrWhiteSpace(addresses)
            ? []
            : addresses.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
    }
}
