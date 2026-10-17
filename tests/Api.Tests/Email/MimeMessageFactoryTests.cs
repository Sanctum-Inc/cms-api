using Application.Common.Models;
using FluentAssertions;
using Infrastructure.Services;
using MimeKit;

namespace Api.Tests.Email;

public class MimeMessageFactoryTests
{
    [Fact]
    public void Create_ShouldMapAllAddressesAndSubject()
    {
        // Arrange
        var message = CreateMessage();

        // Act
        var mimeMessage = MimeMessageFactory.Create(message);

        // Assert
        mimeMessage.From.Mailboxes.Should().ContainSingle(mailbox =>
            mailbox.Address == "sender@lexcase.test" && mailbox.Name == "Test User via LexCase");
        mimeMessage.To.Mailboxes.Should().HaveCount(2);
        mimeMessage.To.Mailboxes.Select(mailbox => mailbox.Address).Should()
            .Equal("recipient1@example.com", "recipient2@example.com");
        mimeMessage.Cc.Mailboxes.Should().ContainSingle(mailbox => mailbox.Address == "cc@example.com");
        mimeMessage.Bcc.Mailboxes.Should().ContainSingle(mailbox => mailbox.Address == "bcc@example.com");
        mimeMessage.ReplyTo.Mailboxes.Should().ContainSingle(mailbox =>
            mailbox.Address == "user@example.com" && mailbox.Name == "Test User");
        mimeMessage.Subject.Should().Be("Subject");
    }

    [Fact]
    public void Create_ShouldUseHtmlTextPart_WhenIsHtml()
    {
        // Arrange
        var message = CreateMessage();

        // Act
        var mimeMessage = MimeMessageFactory.Create(message);

        // Assert
        var body = mimeMessage.Body.Should().BeOfType<TextPart>().Subject;
        body.ContentType.MimeType.Should().Be("text/html");
        body.Text.Should().Be("<p>Body</p>");
    }

    [Fact]
    public void Create_ShouldUsePlainTextPart_WhenNotHtml()
    {
        // Arrange
        var message = CreateMessage();
        message.IsHtml = false;
        message.Body = "Body";

        // Act
        var mimeMessage = MimeMessageFactory.Create(message);

        // Assert
        var body = mimeMessage.Body.Should().BeOfType<TextPart>().Subject;
        body.ContentType.MimeType.Should().Be("text/plain");
        body.Text.Should().Be("Body");
    }

    [Fact]
    public void Create_ShouldOmitReplyTo_WhenNoReplyToAddress()
    {
        // Arrange
        var message = CreateMessage();
        message.ReplyToAddress = null;
        message.ReplyToName = null;

        // Act
        var mimeMessage = MimeMessageFactory.Create(message);

        // Assert
        mimeMessage.ReplyTo.Mailboxes.Should().BeEmpty();
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
}
