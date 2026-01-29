using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Api.Configuration;

public static class PersistenceConfigurator
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment env)
    {
        if (env.IsEnvironment("Test"))
        {
            // Use InMemory database for integration tests
            services.AddDbContext<ApplicationDBContext>(options =>
                options.UseInMemoryDatabase("TestDb")
            );
        }
        else
        {
            // Production / Dev database
            services.AddDbContext<ApplicationDBContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            );
        }

        return services;
    }
}
