// Tests/CustomWebApplicationFactory.cs
using Api.Integration.Tests.Mocks;
using Application.Common.Interfaces.Session;
using Domain.CourtCases;
using Domain.Documents;
using Domain.InvoiceItems;
using Domain.Invoices;
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
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDBContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            // 🔹 Add DbContext with unique in-memory database name per test
            services.AddDbContext<ApplicationDBContext>(options =>
            {
                options.UseInMemoryDatabase(_databaseName);
            });

            // Remove real authentication handlers if needed
            var authDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IAuthenticationSchemeProvider));
            if (authDescriptor != null)
                services.Remove(authDescriptor);

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
            var sessionDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(ISessionResolver));
            if (sessionDescriptor != null)
                services.Remove(sessionDescriptor);

            services.AddScoped<ISessionResolver, SessionResolverMock>();

            services.Configure<DocumentStorageOptions>(options =>
            {
                options.RootPath = Path.Combine(Directory.GetCurrentDirectory(), "Mocks");
            });
        });
    }

    // 🔹 Seed database once during initialization
    public async Task SeedDatabaseAsync()
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();

        // Ensure database is created
        await db.Database.EnsureCreatedAsync();

        // Seed data
        await SeedTestData(db);
    }

    private static async Task SeedTestData(ApplicationDBContext db)
    {
        // Clear existing data (in case of re-seeding)
        db.InvoiceItems.RemoveRange(db.InvoiceItems);
        db.Invoices.RemoveRange(db.Invoices);
        db.CourtCases.RemoveRange(db.CourtCases);
        db.Lawyers.RemoveRange(db.Lawyers);
        db.Users.RemoveRange(db.Users);
        db.Documents.RemoveRange(db.Documents);
        await db.SaveChangesAsync();

        var userId = new Guid("6ec0df63-8960-46ec-9163-2de98e04d5e9");
        var caseId = new Guid("9ae37995-fb0f-4f86-8f9f-30068950df4c");

        // Seed user
        var user = new Domain.Users.User
        {
            Id = userId,
            Name = "testUser",
            Email = "testuser@example.com",
            Surname = "testSurname",
            MobileNumber = "+27812198232",
            PasswordHash = "BFqr1L1tvZ2mmThXw9i13LtCaHa/caTOr/uBMuQ6d/k=",
            PasswordSalt = "qxAHZlcWRdQdB4+Nb+RpTg==",
        };
        db.Users.Add(user);

        // Seed court case
        var courtCase = new Domain.CourtCases.CourtCase
        {
            Id = caseId,
            CaseNumber = "CASE-INT-002",
            Location = "Johannesburg",
            Plaintiff = "John",
            Defendant = "Jane",
            Status = "Active",
            Type = "Criminal",
            Outcome = null,
            UserId = userId
        };
        db.CourtCases.Add(courtCase);

        // Seed invoice
        var invoice = new Invoice
        {
            Id = new Guid("c91d2a2c-1a3e-4a1a-aaa0-1f6b091f7f33"),
            InvoiceNumber = "INV-2025-001",
            InvoiceDate = DateTime.UtcNow,
            ClientName = "John Doe",
            Reference = "REF-001",
            CaseName = "John vs Jane",
            UserId = userId,
            CaseId = caseId,
            AccountName = "Test Account",
            Bank = "ABSA",
            BranchCode = "12345",
            AccountNumber = "987654321",
            CreatedBy = userId,
            Created = DateTime.UtcNow
        };
        db.Invoices.Add(invoice);

        // 🔹 Seed invoice item with correct ID from test
        var invoiceItem = new InvoiceItem
        {
            Id = new Guid("11111111-1111-1111-1111-111111111111"),
            InvoiceId = invoice.Id,
            Name = "Consultation Fee",
            Hours = 2,
            CostPerHour = 1500m,
            IsDayFee = false,
            UserId = userId,
            CreatedBy = userId,
            Created = DateTime.UtcNow,
            DayFeeAmount = null
        };
        db.InvoiceItems.Add(invoiceItem);

        // 🔹 Add second invoice item for delete test
        var invoiceItem2 = new InvoiceItem
        {
            Id = new Guid("44444444-4444-4444-4444-444444444444"),
            InvoiceId = invoice.Id,
            Name = "Court Appearance",
            Hours = 5,
            CostPerHour = 2000m,
            IsDayFee = false,
            UserId = userId,
            CreatedBy = userId,
            Created = DateTime.UtcNow,
            DayFeeAmount = null
        };
        db.InvoiceItems.Add(invoiceItem2);

        var document = new Document()
        {
            Id = new Guid("8f1b1dbf-0c63-4e0e-a16c-4cc78e66ad98"),
            CaseId = caseId,
            ContentType = "text/plain",
            FileName = "test.txt",
            Name = "Test Document",
            Size = 2048,
            UserId = userId,
        };
        db.Documents.Add(document);

        // Seed lawyers
        AddLawyers(db, userId);

        await db.SaveChangesAsync();
    }

    private static void AddLawyers(ApplicationDBContext db, Guid userId)
    {
        db.Lawyers.Add(new Domain.Lawyers.Lawyer
        {
            Id = new Guid("1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d"),
            Email = "james.wilson@lawfirm.com",
            Name = "James",
            Surname = "Wilson",
            MobileNumber = "+27821234567",
            UserId = userId,
            Specialty = Domain.Lawyers.Speciality.CriminalLaw,
            CreatedBy = userId,
            Created = DateTime.UtcNow
        });

        db.Lawyers.Add(new Domain.Lawyers.Lawyer
        {
            Id = new Guid("2b3c4d5e-6f7a-8b9c-0d1e-2f3a4b5c6d7e"),
            Email = "sarah.johnson@lawfirm.com",
            Name = "Sarah",
            Surname = "Johnson",
            MobileNumber = "+27829876543",
            UserId = userId,
            Specialty = Domain.Lawyers.Speciality.FamilyLaw,
            CreatedBy = userId,
            Created = DateTime.UtcNow
        });

        db.Lawyers.Add(new Domain.Lawyers.Lawyer
        {
            Id = new Guid("3c4d5e6f-7a8b-9c0d-1e2f-3a4b5c6d7e8f"),
            Email = "michael.brown@lawfirm.com",
            Name = "Michael",
            Surname = "Brown",
            MobileNumber = "+27835551234",
            UserId = userId,
            Specialty = Domain.Lawyers.Speciality.CorporateLaw,
            CreatedBy = userId,
            Created = DateTime.UtcNow
        });

        db.Lawyers.Add(new Domain.Lawyers.Lawyer
        {
            Id = new Guid("4d5e6f7a-8b9c-0d1e-2f3a-4b5c6d7e8f9a"),
            Email = "emily.davis@lawfirm.com",
            Name = "Emily",
            Surname = "Davis",
            MobileNumber = "+27847778888",
            UserId = userId,
            Specialty = Domain.Lawyers.Speciality.IntellectualPropertyLaw,
            CreatedBy = userId,
            Created = DateTime.UtcNow
        });
    }
}
