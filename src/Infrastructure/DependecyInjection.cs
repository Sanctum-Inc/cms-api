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
using test;

namespace Infrastructure;

public static class DependecyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Register your services here

        AddServices(services);

        AddPersistence(services);

        AddDocumentStorage(services, configuration);

        return services;
    }

    private static void AddDocumentStorage(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DocumentStorageOptions>(
            configuration.GetSection(DocumentStorageOptions.SectionName));
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
    }
}
