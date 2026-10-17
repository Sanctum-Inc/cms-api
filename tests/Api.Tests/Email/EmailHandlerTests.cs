using Application.Common.Interfaces.Services;
using Domain.Emails;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Worker.EmailWorker;
using EmailEntity = Domain.Emails.Email;

namespace Api.Tests.Email;

public class EmailHandlerTests
{
    private readonly Mock<IEmailService> _emailServiceMock = new();
    private readonly Mock<IServiceScopeFactory> _scopeFactoryMock = new();

    public EmailHandlerTests()
    {
        var scopeMock = new Mock<IServiceScope>();
        var serviceProviderMock = new Mock<IServiceProvider>();

        serviceProviderMock
            .Setup(provider => provider.GetService(typeof(IEmailService)))
            .Returns(_emailServiceMock.Object);

        scopeMock
            .Setup(scope => scope.ServiceProvider)
            .Returns(serviceProviderMock.Object);

        _scopeFactoryMock
            .Setup(factory => factory.CreateScope())
            .Returns(scopeMock.Object);
    }

    [Fact]
    public async Task ProcessOnceAsync_ShouldFetchAndSendPendingEmail()
    {
        // Arrange
        var email = CreateEmail();

        _emailServiceMock
            .Setup(service => service.GetUnsentEmails(It.IsAny<CancellationToken>()))
            .ReturnsAsync(email);
        _emailServiceMock
            .Setup(service => service.SendEmails(email, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = CreateHandler();

        // Act
        await handler.ProcessOnceAsync(CancellationToken.None);

        // Assert
        _emailServiceMock.Verify(service => service.GetUnsentEmails(It.IsAny<CancellationToken>()), Times.Once);
        _emailServiceMock.Verify(service => service.SendEmails(email, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ProcessOnceAsync_ShouldForwardCancellationToken()
    {
        // Arrange
        using var cts = new CancellationTokenSource();

        _emailServiceMock
            .Setup(service => service.GetUnsentEmails(cts.Token))
            .ReturnsAsync((EmailEntity?)null);
        _emailServiceMock
            .Setup(service => service.SendEmails(null, cts.Token))
            .Returns(Task.CompletedTask);

        var handler = CreateHandler();

        // Act
        await handler.ProcessOnceAsync(cts.Token);

        // Assert
        _emailServiceMock.Verify(service => service.GetUnsentEmails(cts.Token), Times.Once);
        _emailServiceMock.Verify(service => service.SendEmails(null, cts.Token), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldKeepPollingUntilCancelled()
    {
        // Arrange
        var iterations = 0;

        _emailServiceMock
            .Setup(service => service.GetUnsentEmails(It.IsAny<CancellationToken>()))
            .Callback(() => Interlocked.Increment(ref iterations))
            .ReturnsAsync((EmailEntity?)null);
        _emailServiceMock
            .Setup(service => service.SendEmails(null, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = CreateHandler();
        using var cts = new CancellationTokenSource();

        // Act
        await handler.StartAsync(cts.Token);
        await WaitUntilAsync(() => Volatile.Read(ref iterations) >= 2);
        await handler.StopAsync(CancellationToken.None);

        // Assert
        Volatile.Read(ref iterations).Should().BeGreaterThanOrEqualTo(2);
    }

    private EmailHandler CreateHandler()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["EmailOptions:PollIntervalSeconds"] = "1"
            })
            .Build();

        return new EmailHandler(NullLogger<EmailHandler>.Instance, _scopeFactoryMock.Object, configuration);
    }

    private static async Task WaitUntilAsync(Func<bool> condition, int timeoutMs = 5000)
    {
        var deadline = DateTime.UtcNow.AddMilliseconds(timeoutMs);

        while (!condition() && DateTime.UtcNow < deadline)
        {
            await Task.Delay(10);
        }

        condition().Should().BeTrue("expected the worker loop to keep polling");
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
            UserId = Guid.NewGuid()
        };
    }
}
