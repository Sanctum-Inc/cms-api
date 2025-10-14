using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Api.Configuration;

public static class PersistenceConfigurator
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        // Example: Configure Entity Framework Core with SQL Server
        services.AddDbContext<ApplicationDBContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
        );

        return services;
    }
}
