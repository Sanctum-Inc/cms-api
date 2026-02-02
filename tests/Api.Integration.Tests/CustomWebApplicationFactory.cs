// Tests/CustomWebApplicationFactory.cs

using Api.Integration.Tests.Mocks;
using Application.Common.Interfaces.Session;
using Infrastructure.Config;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Integration.Tests;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    // 🔹 Generate unique database name per factory instance
    private readonly string _databaseName = $"TestDb_{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureServices(services =>
        {
            // Remove existing DbContext registration
            var descriptor =
                services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDBContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // 🔹 Add DbContext with unique in-memory database name per test
            services.AddDbContext<ApplicationDBContext>(options => { options.UseInMemoryDatabase(_databaseName); });

            // Remove real authentication handlers if needed
            var authDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IAuthenticationSchemeProvider));
            if (authDescriptor != null)
            {
                services.Remove(authDescriptor);
            }

            // Add test authentication with default scheme
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = TestAuthHandler.SchemeName;
                    options.DefaultChallengeScheme = TestAuthHandler.SchemeName;
                    options.DefaultScheme = TestAuthHandler.SchemeName;
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    TestAuthHandler.SchemeName, options => { });

            // Add authorization
            services.AddAuthorizationBuilder()
                .AddPolicy("AllowAll", policy =>
                    policy.RequireAssertion(_ => true));

            // Replace ISessionResolver with mock
            var sessionDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ISessionResolver));
            if (sessionDescriptor != null)
            {
                services.Remove(sessionDescriptor);
            }

            services.AddScoped<ISessionResolver, SessionResolverMock>();

            services.Configure<DocumentStorageOptions>(options =>
            {
                options.RootPath = Path.Combine(Directory.GetCurrentDirectory(), "Mocks");
            });
        });
    }

    internal async Task SeedDatabaseAsync()
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>(); // Ensure database is created
        await db.Database.EnsureCreatedAsync(); // Seed data
        await TestDataSeeder.SeedDatabaseAsync(db);
    }
}
