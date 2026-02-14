using Application.Common.Interfaces.Services;

namespace Worker.EmailWorker;

public class EmailHandler : BackgroundService
{
    private readonly ILogger<EmailHandler> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public EmailHandler(ILogger<EmailHandler> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            using var scope = _scopeFactory.CreateScope();

            var emailService = scope.ServiceProvider
                .GetRequiredService<IEmailService>();

            var result = await emailService.GetUnsentEmails(stoppingToken);

            await emailService.SendEmails(result, stoppingToken);

            await Task.Delay(1000, stoppingToken);
        }
    }
}
