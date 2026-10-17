using Application.Common.Interfaces.Services;
using Application.Common.Models;
using Infrastructure.Config;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class SmtpEmailSender : IEmailSender
{
    private readonly EmailOptions _emailOptions;

    public SmtpEmailSender(IOptions<EmailOptions> options)
    {
        _emailOptions = options.Value;
    }

    public async Task SendAsync(EmailMessage message, CancellationToken cancellationToken)
    {
        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(_emailOptions.Host, _emailOptions.Port, SecureSocketOptions.StartTls,
                cancellationToken);

            if (!string.IsNullOrWhiteSpace(_emailOptions.Username))
            {
                await client.AuthenticateAsync(_emailOptions.Username, _emailOptions.Password, cancellationToken);
            }

            await client.SendAsync(MimeMessageFactory.Create(message), cancellationToken);
        }
        finally
        {
            await client.DisconnectAsync(true, cancellationToken);
        }
    }
}
