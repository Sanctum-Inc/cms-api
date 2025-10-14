using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Common.Interfaces.Session;
using Infrastructure.Common;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;
public static class DependecyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Register your services here

        AddServices(services);

        AddPersistence(services);

        return services;
    }

    private static void AddPersistence(IServiceCollection services)
    {
        services.AddScoped<IApplicationDBContext, ApplicationDBContext>();
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<ICourtCaseRepository, CourtCaseRepository>();
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddScoped<ISessionResolver, SessionResolver>();
        services.AddScoped<ICourtCaseService, CourtCaseService>();
    }
}
