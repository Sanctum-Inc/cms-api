using Application.Common.Interfaces.Persistence;
using Domain.CourtCaseDates;
using Domain.Users;
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

                var userType = db.Model.FindEntityType(typeof(User));
                foreach (var fk in userType.GetForeignKeys())
                {
                    Console.WriteLine($"User FK to {fk.PrincipalEntityType.Name}: {fk.DeleteBehavior}");
                }

                var courtCaseDateType = db.Model.FindEntityType(typeof(CourtCaseDate));
                foreach (var fk in courtCaseDateType.GetForeignKeys())
                {
                    Console.WriteLine($"CourtCaseDate FK to {fk.PrincipalEntityType.Name}: {fk.DeleteBehavior}");
                }
                db.Database.Migrate();

            }

        }
        return app;
    }
}
