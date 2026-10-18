using Amazon.Runtime;
using Amazon.S3;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Common.Interfaces.Session;
using Infrastructure.Common;
using Infrastructure.Config;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using test;

namespace Infrastructure;

public static class DependecyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Register your services here

        AddServices(services);

        AddPersistence(services);

        AddOptions(services, configuration);

        AddEmailSender(services, configuration);

        AddFileStorage(services, configuration);

        return services;
    }

    private static void AddEmailSender(IServiceCollection services, IConfiguration configuration)
    {
        var provider = configuration[$"{EmailOptions.SectionName}:{nameof(EmailOptions.Provider)}"];

        if (string.Equals(provider, EmailProviders.Brevo, StringComparison.OrdinalIgnoreCase))
        {
            services.AddHttpClient<BrevoEmailSender>();
            services.AddTransient<IEmailSender>(serviceProvider =>
                serviceProvider.GetRequiredService<BrevoEmailSender>());
            return;
        }

        services.AddScoped<IEmailSender, SmtpEmailSender>();
    }

    private static void AddFileStorage(IServiceCollection services, IConfiguration configuration)
    {
        var provider = configuration[$"{DocumentStorageOptions.SectionName}:{nameof(DocumentStorageOptions.Provider)}"];

        if (string.Equals(provider, DocumentStorageProviders.S3, StringComparison.OrdinalIgnoreCase))
        {
            services.AddSingleton<IAmazonS3>(serviceProvider =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<DocumentStorageOptions>>().Value;
                return new AmazonS3Client(
                    new BasicAWSCredentials(options.S3.AccessKeyId, options.S3.SecretAccessKey),
                    new AmazonS3Config
                    {
                        ServiceURL = options.S3.Endpoint,
                        ForcePathStyle = true,
                        AuthenticationRegion = "auto"
                    });
            });
            services.AddSingleton<IFileStorage, S3FileStorage>();
            return;
        }

        services.AddSingleton<IFileStorage, LocalFileStorage>();
    }

    private static void AddOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DocumentStorageOptions>(
            configuration.GetSection(DocumentStorageOptions.SectionName));
        services.Configure<EmailOptions>(
            configuration.GetSection(EmailOptions.SectionName));
        services.Configure<EnvironmentOptions>(
            configuration.GetSection(EnvironmentOptions.SectionName));
        services.Configure<JwtOptions>(
            configuration.GetSection(JwtOptions.SectionName));
    }

    private static void AddPersistence(IServiceCollection services)
    {
        services.AddScoped<IApplicationDBContext, ApplicationDBContext>();
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<ICourtCaseRepository, CourtCaseRepository>();
        services.AddScoped<IDocumentRepository, DocumentRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ILawyerRepository, LawyerRepository>();
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        services.AddScoped<IFirmRepository, FirmRepository>();
        services.AddScoped<IInvoiceItemRepository, InvoiceItemRepository>();
        services.AddScoped<ICourtCaseDateRepository, CourtCaseDateRepository>();
        services.AddScoped<IEmailRepository, EmailRepository>();
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddScoped<ISessionResolver, SessionResolver>();
        services.AddScoped<ICourtCaseService, CourtCaseService>();
        services.AddScoped<IDocumentService, DocumentService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<ILawyerService, LawyerService>();
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<IInvoiceItemService, InvoiceItemService>();
        services.AddScoped<ICourtCaseDatesService, CourtCaseDatesService>();
        services.AddScoped<IFirmService, FirmService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IPdfService, PdfService>();
    }
}
