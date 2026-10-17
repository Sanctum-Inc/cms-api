using System.Net.Http.Json;
using Application.Common.Interfaces.Services;
using Application.Common.Models;
using Infrastructure.Config;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class BrevoEmailSender : IEmailSender
{
    public const string SendEmailUrl = "https://api.brevo.com/v3/smtp/email";

    private readonly HttpClient _httpClient;
    private readonly EmailOptions _emailOptions;

    public BrevoEmailSender(HttpClient httpClient, IOptions<EmailOptions> options)
    {
        _httpClient = httpClient;
        _emailOptions = options.Value;
    }

    public async Task SendAsync(EmailMessage message, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_emailOptions.ApiKey))
        {
            throw new InvalidOperationException(
                $"{EmailOptions.SectionName}:ApiKey is required when using the {EmailProviders.Brevo} provider.");
        }

        using var request = new HttpRequestMessage(HttpMethod.Post, SendEmailUrl);
        request.Headers.Add("api-key", _emailOptions.ApiKey);
        request.Content = JsonContent.Create(BuildPayload(message));

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException(
                $"Brevo send failed with status {(int)response.StatusCode}: {error}");
        }
    }

    public static Dictionary<string, object> BuildPayload(EmailMessage message)
    {
        var payload = new Dictionary<string, object>
        {
            ["sender"] = new { name = message.FromName, email = message.FromAddress },
            ["to"] = message.To.Select(address => new { email = address }).ToList(),
            ["subject"] = message.Subject
        };

        if (message.Cc.Count > 0)
        {
            payload["cc"] = message.Cc.Select(address => new { email = address }).ToList();
        }

        if (message.Bcc.Count > 0)
        {
            payload["bcc"] = message.Bcc.Select(address => new { email = address }).ToList();
        }

        if (!string.IsNullOrWhiteSpace(message.ReplyToAddress))
        {
            payload["replyTo"] = new
            {
                name = message.ReplyToName ?? message.ReplyToAddress,
                email = message.ReplyToAddress
            };
        }

        if (message.IsHtml)
        {
            payload["htmlContent"] = message.Body;
        }
        else
        {
            payload["textContent"] = message.Body;
        }

        return payload;
    }
}
