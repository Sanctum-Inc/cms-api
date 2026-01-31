using Domain.CourtCaseDates;
using Domain.CourtCases;
using Domain.Documents;
using Domain.InvoiceItems;
using Domain.Invoices;
using Domain.Lawyers;
using Domain.Users;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Api.Integration.Tests.Mocks;

public static class TestDataSeeder
{
    public static async Task SeedDatabaseAsync(ApplicationDBContext db)
    {
        // Clear existing data
        await ClearDatabaseAsync(db);

        // Seed all entities
        var user = await SeedUsersAsync(db);
        var courtCase = await SeedCourtCasesAsync(db, user.Id);
        await SeedInvoicesAsync(db, user.Id, courtCase.Id);
        await SeedDocumentsAsync(db, user.Id, courtCase.Id);
        await SeedLawyersAsync(db, user.Id);
        await SeedCourtCaseDates(db, user.Id);

        await db.SaveChangesAsync();
    }

    private static async Task SeedCourtCaseDates(ApplicationDBContext db, Guid userId)
    {

        // 2️⃣ Create a CourtCase for that user
        var caseId = Guid.Parse("9ae37995-fb0f-4f86-8f9f-30068950df4c");

        if (!await db.CourtCases.AnyAsync(c => c.Id == caseId))
        {
            var courtCase = new CourtCase
            {
                Id = caseId,
                CaseNumber = "CC-2025-001",
                Location = "Johannesburg High Court",
                Plaintiff = "John Doe",
                Defendant = "State of South Africa",
                Status = CourtCaseStatus.Draft,
                Type = "Criminal",
                Outcome = "Pending",
                UserId = userId,
                Created = DateTime.UtcNow,
                IsPaid = false,
            };

            await db.CourtCases.AddAsync(courtCase);

            // 3️⃣ Add at least one CourtCaseDate linked to that case
            var courtCaseDate = new CourtCaseDate
            {
                Id = Guid.NewGuid(),
                Date = "2025-10-31",
                Title = "Initial Hearing",
                CaseId = courtCase.Id,
                Case = courtCase,
                Created = DateTime.UtcNow,
                UserId = userId,
                Type = courtCase.Type,
            };

            await db.CourtCaseDates.AddAsync(courtCaseDate);
        }

        // 4️⃣ Save all seeded entities
        await db.SaveChangesAsync();
    }


    private static async Task ClearDatabaseAsync(ApplicationDBContext db)
    {
        db.InvoiceItems.RemoveRange(db.InvoiceItems);
        db.Invoices.RemoveRange(db.Invoices);
        db.CourtCases.RemoveRange(db.CourtCases);
        db.Lawyers.RemoveRange(db.Lawyers);
        db.Users.RemoveRange(db.Users);
        db.Documents.RemoveRange(db.Documents);
        db.CourtCaseDates.RemoveRange(db.CourtCaseDates);

        await db.SaveChangesAsync();
    }

    private static async Task<User> SeedUsersAsync(ApplicationDBContext db)
    {
        var user = new User
        {
            Id = new Guid("6ec0df63-8960-46ec-9163-2de98e04d5e9"),
            Name = "testUser",
            Surname = "testSurname",
            Email = "testuser@example.com",
            MobileNumber = "+27812198232",
            PasswordHash = "BFqr1L1tvZ2mmThXw9i13LtCaHa/caTOr/uBMuQ6d/k=",
            PasswordSalt = "qxAHZlcWRdQdB4+Nb+RpTg==",
            FirmId = Guid.NewGuid(),
            Role = UserRole.FirmUser
        };
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return user;
    }

    private static async Task<CourtCase> SeedCourtCasesAsync(ApplicationDBContext db, Guid userId)
    {
        var courtCase = new CourtCase
        {
            Id = new Guid("9ae37995-fb0f-4f86-8f9f-30068950df4c"),
            CaseNumber = "CASE-INT-002",
            Location = "Johannesburg",
            Plaintiff = "John",
            Defendant = "Jane",
            Status = CourtCaseStatus.Draft,
            Type = "Criminal",
            UserId = userId,
            IsPaid = false,
        };
        db.CourtCases.Add(courtCase);
        await db.SaveChangesAsync();
        return courtCase;
    }

    private static async Task SeedInvoicesAsync(ApplicationDBContext db, Guid userId, Guid caseId)
    {
        // Delete existing invoices first
        db.InvoiceItems.RemoveRange(db.InvoiceItems);
        db.Invoices.RemoveRange(db.Invoices);
        await db.SaveChangesAsync();

        var invoice = new Invoice
        {
            Id = new Guid("c91d2a2c-1a3e-4a1a-aaa0-1f6b091f7f33"),
            InvoiceNumber = "INV-2025-001",
            InvoiceDate = DateTime.UtcNow,
            ClientName = "John Doe",
            Reference = "REF-001",
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

        // Invoice items
        var invoiceItem1 = new InvoiceItem
        {
            Id = new Guid("11111111-1111-1111-1111-111111111111"),
            InvoiceId = invoice.Id,
            Name = "Consultation Fee",
            Hours = 2,
            CostPerHour = 1500m,
            UserId = userId,
            CreatedBy = userId,
            Created = DateTime.UtcNow
        };
        var invoiceItem2 = new InvoiceItem
        {
            Id = new Guid("44444444-4444-4444-4444-444444444444"),
            InvoiceId = invoice.Id,
            Name = "Court Appearance",
            Hours = 5,
            CostPerHour = 2000m,
            UserId = userId,
            CreatedBy = userId,
            Created = DateTime.UtcNow
        };
        db.InvoiceItems.AddRange(invoiceItem1, invoiceItem2);

        await db.SaveChangesAsync();
    }

    private static async Task SeedDocumentsAsync(ApplicationDBContext db, Guid userId, Guid caseId)
    {
        var document = new Document
        {
            Id = new Guid("8f1b1dbf-0c63-4e0e-a16c-4cc78e66ad98"),
            CaseId = caseId,
            ContentType = "text/plain",
            FileName = "test.txt",
            Name = "Test Document",
            Size = 2048,
            UserId = userId
        };
        db.Documents.Add(document);
        await db.SaveChangesAsync();
    }

    private static async Task SeedLawyersAsync(ApplicationDBContext db, Guid userId)
    {
        var lawyers = new List<Lawyer>
        {
            new Lawyer
            {
                Id = new Guid("1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d"),
                Email = "james.wilson@lawfirm.com",
                Name = "James",
                Surname = "Wilson",
                MobileNumber = "+27821234567",
                CreatedByUserId = userId,
                Specialty = Speciality.Corporate,
                CreatedBy = userId,
                Created = DateTime.UtcNow
            },
            new Lawyer
            {
                Id = new Guid("2b3c4d5e-6f7a-8b9c-0d1e-2f3a4b5c6d7e"),
                Email = "sarah.johnson@lawfirm.com",
                Name = "Sarah",
                Surname = "Johnson",
                MobileNumber = "+27829876543",
                CreatedByUserId = userId,
                Specialty = Speciality.Corporate,
                CreatedBy = userId,
                Created = DateTime.UtcNow
            },
            new Lawyer
            {
                Id = new Guid("3c4d5e6f-7a8b-9c0d-1e2f-3a4b5c6d7e8f"),
                Email = "michael.brown@lawfirm.com",
                Name = "Michael",
                Surname = "Brown",
                MobileNumber = "+27835551234",
                CreatedByUserId = userId,
                Specialty = Speciality.Environmental,
                CreatedBy = userId,
                Created = DateTime.UtcNow
            },
            new Lawyer
            {
                Id = new Guid("4d5e6f7a-8b9c-0d1e-2f3a-4b5c6d7e8f9a"),
                Email = "emily.davis@lawfirm.com",
                Name = "Emily",
                Surname = "Davis",
                MobileNumber = "+27847778888",
                CreatedByUserId = userId,
                Specialty = Speciality.RealEstate,
                CreatedBy = userId,
                Created = DateTime.UtcNow
            }
        };
        db.Lawyers.AddRange(lawyers);
        await db.SaveChangesAsync();
    }
}
