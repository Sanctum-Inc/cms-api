using Application.Common.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace Worker.EmailWorker;

public class EmailHandler : BackgroundService
{
    private const string PollIntervalKey = "EmailOptions:PollIntervalSeconds";

    private readonly ILogger<EmailHandler> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly TimeSpan _pollInterval;

    public EmailHandler(ILogger<EmailHandler> logger, IServiceScopeFactory scopeFactory, IConfiguration configuration)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _pollInterval = int.TryParse(configuration[PollIntervalKey], out var seconds) && seconds > 0
            ? TimeSpan.FromSeconds(seconds)
            : TimeSpan.FromSeconds(1);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            await ProcessOnceAsync(stoppingToken);

            await Task.Delay(_pollInterval, stoppingToken);
        }
    }

    public async Task ProcessOnceAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();

        var emailService = scope.ServiceProvider
            .GetRequiredService<IEmailService>();

        var result = await emailService.GetUnsentEmails(cancellationToken);

        await emailService.SendEmails(result, cancellationToken);
    }
}
