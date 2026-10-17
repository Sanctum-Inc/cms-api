using System.Net;
using System.Text.Json;
using Application.Common.Models;
using FluentAssertions;
using Infrastructure.Config;
using Infrastructure.Services;
using Microsoft.Extensions.Options;

namespace Api.Tests.Email;

public class BrevoEmailSenderTests
{
    [Fact]
    public async Task SendAsync_ShouldPostToBrevoWithApiKeyHeader()
    {
        // Arrange
        var handler = new StubHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.Created));
        var sender = CreateSender(handler);

        // Act
        await sender.SendAsync(CreateMessage(), CancellationToken.None);

        // Assert
        handler.LastRequest.Should().NotBeNull();
        handler.LastRequest!.Method.Should().Be(HttpMethod.Post);
        handler.LastRequest.RequestUri!.ToString().Should().Be(BrevoEmailSender.SendEmailUrl);
        handler.LastRequest.Headers.GetValues("api-key").Should().BeEquivalentTo("test-api-key");
    }

    [Fact]
    public async Task SendAsync_ShouldMapHtmlMessageToHtmlContent()
    {
        // Arrange
        var handler = new StubHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.Created));
        var sender = CreateSender(handler);

        // Act
        await sender.SendAsync(CreateMessage(), CancellationToken.None);

        // Assert
        using var document = JsonDocument.Parse(handler.LastRequestBody!);
        var root = document.RootElement;

        root.GetProperty("sender").GetProperty("name").GetString().Should().Be("Test User via LexCase");
        root.GetProperty("sender").GetProperty("email").GetString().Should().Be("sender@lexcase.test");
        root.GetProperty("to").GetArrayLength().Should().Be(2);
        root.GetProperty("cc").GetArrayLength().Should().Be(1);
        root.GetProperty("bcc").GetArrayLength().Should().Be(1);
        root.GetProperty("replyTo").GetProperty("email").GetString().Should().Be("user@example.com");
        root.GetProperty("subject").GetString().Should().Be("Subject");
        root.GetProperty("htmlContent").GetString().Should().Be("<p>Body</p>");
        root.TryGetProperty("textContent", out _).Should().BeFalse();
    }

    [Fact]
    public async Task SendAsync_ShouldMapPlainMessageToTextContent()
    {
        // Arrange
        var handler = new StubHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.Created));
        var sender = CreateSender(handler);
        var message = CreateMessage();
        message.IsHtml = false;
        message.Body = "Body";
        message.Cc = [];
        message.Bcc = [];

        // Act
        await sender.SendAsync(message, CancellationToken.None);

        // Assert
        using var document = JsonDocument.Parse(handler.LastRequestBody!);
        var root = document.RootElement;

        root.GetProperty("textContent").GetString().Should().Be("Body");
        root.TryGetProperty("htmlContent", out _).Should().BeFalse();
        root.TryGetProperty("cc", out _).Should().BeFalse();
        root.TryGetProperty("bcc", out _).Should().BeFalse();
    }

    [Fact]
    public async Task SendAsync_ShouldThrowWithResponseBody_WhenBrevoReturnsError()
    {
        // Arrange
        var handler = new StubHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent("{\"code\":\"invalid_parameter\",\"message\":\"sender not valid\"}")
        });
        var sender = CreateSender(handler);

        // Act
        var act = () => sender.SendAsync(CreateMessage(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>()
            .WithMessage("*400*")
            .WithMessage("*sender not valid*");
    }

    [Fact]
    public async Task SendAsync_ShouldThrowWithoutCallingBrevo_WhenApiKeyIsMissing()
    {
        // Arrange
        var handler = new StubHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.Created));
        var sender = CreateSender(handler, apiKey: string.Empty);

        // Act
        var act = () => sender.SendAsync(CreateMessage(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*ApiKey*");
        handler.LastRequest.Should().BeNull();
    }

    private static BrevoEmailSender CreateSender(StubHttpMessageHandler handler, string apiKey = "test-api-key")
    {
        var httpClient = new HttpClient(handler);
        var options = Options.Create(new EmailOptions
        {
            ApiKey = apiKey,
            EmailAddress = "sender@lexcase.test"
        });

        return new BrevoEmailSender(httpClient, options);
    }

    private static EmailMessage CreateMessage()
    {
        return new EmailMessage
        {
            FromName = "Test User via LexCase",
            FromAddress = "sender@lexcase.test",
            To = ["recipient1@example.com", "recipient2@example.com"],
            Cc = ["cc@example.com"],
            Bcc = ["bcc@example.com"],
            ReplyToName = "Test User",
            ReplyToAddress = "user@example.com",
            Subject = "Subject",
            Body = "<p>Body</p>",
            IsHtml = true
        };
    }

    private sealed class StubHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage _response;

        public StubHttpMessageHandler(HttpResponseMessage response)
        {
            _response = response;
        }

        public HttpRequestMessage? LastRequest { get; private set; }

        public string? LastRequestBody { get; private set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            LastRequest = request;
            LastRequestBody = request.Content is null
                ? null
                : await request.Content.ReadAsStringAsync(cancellationToken);

            return _response;
        }
    }
}
