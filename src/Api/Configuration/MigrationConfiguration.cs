using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Api.Configuration;

public static class MigrationConfiguration
{
    public static WebApplication ExecutePendingMigrations(this WebApplication app, IWebHostEnvironment environment)
    {
        if (!environment.IsEnvironment("Test"))
        {
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();

                if (db.Database.IsRelational())
                {
                    db.Database.EnsureCreated();
                }
            }
        }

        return app;
    }
}
