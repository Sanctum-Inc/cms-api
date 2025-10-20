// Tests/CustomWebApplicationFactory.cs
using Api.Integration.Tests.Mocks;
using Application.Common.Interfaces.Session;
using Infrastructure.Config;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Integration.Tests;
public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureServices(services =>
        {
            // 🔸 Remove real authentication handlers if needed
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IAuthenticationSchemeProvider));
            if (descriptor != null)
                services.Remove(descriptor);

            // ✅ Add our test authentication
            services.AddAuthentication(TestAuthHandler.SchemeName)
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    TestAuthHandler.SchemeName, options => { });

            services.AddAuthorizationBuilder()
                .AddPolicy("AllowAll", policy =>
                    policy.RequireAssertion(_ => true));

            services.Configure<DocumentStorageOptions>(options =>
            {
                options.RootPath = Path.Combine(Directory.GetCurrentDirectory(), "Mocks");
            });


            // Build the service provider and seed data
            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
            db.Database.EnsureCreated();
        });

    }

    private static async Task SeedTestData(ApplicationDBContext db)
    {
        // clear users to avoid duplicate key error
        db.Users.RemoveRange(db.Users);
        db.Documents.RemoveRange(db.Documents);
        db.Lawyers.RemoveRange(db.Lawyers);
        await db.SaveChangesAsync();

        // Example seed
        db.Users.Add(new Domain.Users.User
        {
            Id = new Guid("6ec0df63-8960-46ec-9163-2de98e04d5e9"),
            Name = "testUser",
            Email = "testuser@example.com",
            Surname = "testSurname",
            MobileNumber = "+27812198232",
            PasswordHash = "BFqr1L1tvZ2mmThXw9i13LtCaHa/caTOr/uBMuQ6d/k=",
            PasswordSalt = "qxAHZlcWRdQdB4+Nb+RpTg==",
        });

        db.CourtCases.Add(new Domain.CourtCases.CourtCase
        {
            Id = new Guid("9ae37995-fb0f-4f86-8f9f-30068950df4c"),
            CaseNumber= "CASE-INT-002",
            Location= "Johannesburg",
            Plaintiff= "John",
            Defendant= "Jane",
            Status= "Active",
            Type= "Criminal",
            Outcome= null,
            UserId = new Guid("6ec0df63-8960-46ec-9163-2de98e04d5e9"),
        });

        db.Documents.Add(new Domain.Documents.Document
        {
            Id = new Guid("8f1b1dbf-0c63-4e0e-a16c-4cc78e66ad98"),
            Name = "Test Document",
            FileName = "test.txt",
            ContentType = "text/plain",
            Size = 1024,
            UserId = new Guid("6ec0df63-8960-46ec-9163-2de98e04d5e9"),
            CaseId = new Guid("9ae37995-fb0f-4f86-8f9f-30068950df4c"),
            CreatedBy = Guid.NewGuid()
        });

        // Add these lawyers to your database seeding
        db.Lawyers.Add(new Domain.Lawyers.Lawyer
        {
            Id = new Guid("1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d"),
            Email = "james.wilson@lawfirm.com",
            Name = "James",
            Surname = "Wilson",
            MobileNumber = "+27821234567",
            UserId = new Guid("6ec0df63-8960-46ec-9163-2de98e04d5e9"),
            Specialty = Domain.Lawyers.Speciality.CriminalLaw,
            CreatedBy = Guid.NewGuid(),
            Created = DateTime.UtcNow
        });

        db.Lawyers.Add(new Domain.Lawyers.Lawyer
        {
            Id = new Guid("2b3c4d5e-6f7a-8b9c-0d1e-2f3a4b5c6d7e"),
            Email = "sarah.johnson@lawfirm.com",
            Name = "Sarah",
            Surname = "Johnson",
            MobileNumber = "+27829876543",
            UserId = new Guid("6ec0df63-8960-46ec-9163-2de98e04d5e9"),
            Specialty = Domain.Lawyers.Speciality.FamilyLaw,
            CreatedBy = Guid.NewGuid(),
            Created = DateTime.UtcNow
        });

        db.Lawyers.Add(new Domain.Lawyers.Lawyer
        {
            Id = new Guid("3c4d5e6f-7a8b-9c0d-1e2f-3a4b5c6d7e8f"),
            Email = "michael.brown@lawfirm.com",
            Name = "Michael",
            Surname = "Brown",
            MobileNumber = "+27835551234",
            UserId = new Guid("6ec0df63-8960-46ec-9163-2de98e04d5e9"),
            Specialty = Domain.Lawyers.Speciality.CorporateLaw,
            CreatedBy = Guid.NewGuid(),
            Created = DateTime.UtcNow
        });

        db.Lawyers.Add(new Domain.Lawyers.Lawyer
        {
            Id = new Guid("4d5e6f7a-8b9c-0d1e-2f3a-4b5c6d7e8f9a"),
            Email = "emily.davis@lawfirm.com",
            Name = "Emily",
            Surname = "Davis",
            MobileNumber = "+27847778888",
            UserId = new Guid("6ec0df63-8960-46ec-9163-2de98e04d5e9"),
            Specialty = Domain.Lawyers.Speciality.IntellectualPropertyLaw,
            CreatedBy = Guid.NewGuid(),
            Created = DateTime.UtcNow
        });

        await db.SaveChangesAsync();
    }

    public async Task ResetDatabase(ApplicationDBContext db)
    {
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
        await SeedTestData(db);
    }

    public void Dispose(ApplicationDBContext db)
    {
        db.Dispose();
    }
}
