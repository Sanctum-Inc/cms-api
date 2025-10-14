using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Api.Configuration
{
    public class PersistenceConfigurator
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Example: Configure Entity Framework Core with SQL Server
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
);
        }
    }
}
