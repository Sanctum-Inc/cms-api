using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Common.Interfaces.Session;
using Application.Common.Models;
using Domain.Emails;
using Domain.Users;
using FluentAssertions;
using Infrastructure.Config;
using Infrastructure.Services;
using MapsterMapper;
using Microsoft.Extensions.Options;
using Moq;
using EmailEntity = Domain.Emails.Email;

namespace Api.Tests.Email;

public class EmailServiceTests
{
    private readonly Mock<IEmailRepository> _repositoryMock = new();
    private readonly Mock<IEmailSender> _senderMock = new();
    private readonly EmailService _service;

    public EmailServiceTests()
    {
        var mapperMock = new Mock<IMapper>();
        var sessionResolverMock = new Mock<ISessionResolver>();
        var options = Options.Create(new EmailOptions
        {
            EmailAddress = "sender@lexcase.test",
            EmailName = "LexCase"
        });

        _repositoryMock
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _service = new EmailService(mapperMock.Object, _repositoryMock.Object, sessionResolverMock.Object, options,
            _senderMock.Object);
    }

    [Fact]
    public async Task SendEmails_ShouldSendAndMarkSent_WhenSenderSucceeds()
    {
        // Arrange
        var email = CreateEmail();
        EmailMessage? captured = null;

        _senderMock
            .Setup(s => s.SendAsync(It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>()))
            .Callback<EmailMessage, CancellationToken>((message, _) => captured = message)
            .Returns(Task.CompletedTask);

        // Act
        await _service.SendEmails(email, CancellationToken.None);

        // Assert
        email.Status.Should().Be(EmailStatus.Sent);
        email.SentAt.Should().NotBeNull();
        captured.Should().NotBeNull();
        captured!.FromAddress.Should().Be("sender@lexcase.test");
        captured.Subject.Should().Be("Subject");
        captured.IsHtml.Should().BeTrue();
        _senderMock.Verify(s => s.SendAsync(It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SendEmails_ShouldMarkFailedWithoutSending_WhenUserIsNull()
    {
        // Arrange
        var email = CreateEmail();
        email.User = null;

        // Act
        await _service.SendEmails(email, CancellationToken.None);

        // Assert
        email.Status.Should().Be(EmailStatus.Failed);
        email.ErrorMessage.Should().Be("No user linked to this email.");
        _senderMock.Verify(s => s.SendAsync(It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task SendEmails_ShouldRetryThreeTimesThenFail_WhenSenderAlwaysThrows()
    {
        // Arrange
        var email = CreateEmail();

        _senderMock
            .Setup(s => s.SendAsync(It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("smtp-port-blocked"));

        // Act
        await _service.SendEmails(email, CancellationToken.None);

        // Assert
        email.Status.Should().Be(EmailStatus.Failed);
        email.RetryCount.Should().Be(3);
        email.ErrorMessage.Should().Contain("smtp-port-blocked");
        _senderMock.Verify(s => s.SendAsync(It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>()), Times.Exactly(4));
    }

    [Fact]
    public async Task SendEmails_ShouldMapToCcBccAndReplyTo_TrimmingAddresses()
    {
        // Arrange
        var email = CreateEmail();
        email.ToAddresses = " to1@example.com ; to2@example.com";
        email.CcAddresses = "cc1@example.com; cc2@example.com";
        email.BccAddresses = " bcc@example.com ";

        EmailMessage? captured = null;

        _senderMock
            .Setup(s => s.SendAsync(It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>()))
            .Callback<EmailMessage, CancellationToken>((message, _) => captured = message)
            .Returns(Task.CompletedTask);

        // Act
        await _service.SendEmails(email, CancellationToken.None);

        // Assert
        captured!.To.Should().Equal("to1@example.com", "to2@example.com");
        captured.Cc.Should().Equal("cc1@example.com", "cc2@example.com");
        captured.Bcc.Should().Equal("bcc@example.com");
        captured.ReplyToAddress.Should().Be("user@example.com");
        captured.ReplyToName.Should().Be("Test User");
    }

    [Fact]
    public async Task SendEmails_ShouldDoNothing_WhenEmailIsNull()
    {
        // Act
        await _service.SendEmails(null, CancellationToken.None);

        // Assert
        _senderMock.Verify(s => s.SendAsync(It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>()), Times.Never);
        _repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    private static EmailEntity CreateEmail()
    {
        return new EmailEntity
        {
            Id = Guid.NewGuid(),
            ToAddresses = "recipient@example.com",
            Subject = "Subject",
            Body = "<p>Body</p>",
            IsHtml = true,
            RetryCount = 0,
            Status = EmailStatus.Pending,
            UserId = Guid.NewGuid(),
            User = new User
            {
                Email = "user@example.com",
                Name = "Test",
                Surname = "User",
                MobileNumber = "0123456789",
                PasswordHash = "hash",
                PasswordSalt = "salt",
                Role = UserRole.FirmUser,
                IsEmailVerified = true,
                FirmId = Guid.NewGuid()
            }
        };
    }
}
