using Bogus;
using ConsoleHelpers;
using Domain.CourtCaseDates;
using Domain.CourtCases;
using Domain.Documents;
using Domain.Firms;
using Domain.InvoiceItems;
using Domain.Invoices;
using Domain.Lawyers;
using Domain.Users;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

Console.WriteLine("=== Court Management System - Seed Data Generator ===\n");

// Get number of records to generate
Console.Write("How many firms do you want to generate? (default: 5): ");
var firmsCount = int.TryParse(Console.ReadLine(), out var f) ? f : 5;

Console.Write("How many users per firm? (default: 10): ");
var usersPerFirm = int.TryParse(Console.ReadLine(), out var u) ? u : 10;

Console.Write("How many court cases per user? (default: 5): ");
var casesPerUser = int.TryParse(Console.ReadLine(), out var c) ? c : 5;

Console.WriteLine($"\nGenerating:");
Console.WriteLine($"  - {firmsCount} firms");
Console.WriteLine($"  - {firmsCount * usersPerFirm} users ({usersPerFirm} per firm)");
Console.WriteLine($"  - ~{firmsCount * usersPerFirm * casesPerUser} court cases");
Console.WriteLine($"  - Associated dates, documents, invoices, and lawyers\n");

Console.Write("Proceed? (y/n): ");
if (Console.ReadLine()?.ToLower() != "y")
{
    Console.WriteLine("Cancelled.");
    return;
}

// Setup database connection
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var connectionString = configuration.GetConnectionString("DefaultConnection");

var optionsBuilder = new DbContextOptionsBuilder<ApplicationDBContext>();
optionsBuilder.UseSqlServer(connectionString);

using var context = new ApplicationDBContext(optionsBuilder.Options);

// Ensure database exists
Console.WriteLine("\nEnsuring database exists...");
await context.Database.EnsureCreatedAsync();

// Generate seed data
var generator = new SeedDataGenerator(context);
await generator.GenerateAsync(firmsCount, usersPerFirm, casesPerUser);

Console.WriteLine("\n✅ Seed data generation complete!");
Console.WriteLine("Press any key to exit...");
Console.ReadKey();

namespace ConsoleHelpers
{
    public class SeedDataGenerator
    {
        private readonly ApplicationDBContext _context;
        private readonly Random _random = new Random();
        private readonly Faker _faker = new Faker();

        public SeedDataGenerator(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task GenerateAsync(int firmsCount, int usersPerFirm, int casesPerUser)
        {
            Console.WriteLine("\n🏢 Generating Firms...");
            var firms = GenerateFirms(firmsCount);
            await _context.Firms.AddRangeAsync(firms);
            await _context.SaveChangesAsync();
            Console.WriteLine($"   Created {firms.Count} firms");

            Console.WriteLine("\n👤 Generating Users...");
            var users = GenerateUsers(firms, usersPerFirm);
            await _context.Users.AddRangeAsync(users);
            await _context.SaveChangesAsync();
            Console.WriteLine($"   Created {users.Count} users");

            Console.WriteLine("\n⚖️ Generating External Lawyers...");
            var lawyers = GenerateLawyers(users);
            await _context.Lawyers.AddRangeAsync(lawyers);
            await _context.SaveChangesAsync();
            Console.WriteLine($"   Created {lawyers.Count} external lawyers");

            Console.WriteLine("\n📋 Generating Court Cases...");
            var courtCases = GenerateCourtCases(users, casesPerUser);
            await _context.CourtCases.AddRangeAsync(courtCases);
            await _context.SaveChangesAsync();
            Console.WriteLine($"   Created {courtCases.Count} court cases");

            Console.WriteLine("\n📅 Generating Court Case Dates...");
            var courtCaseDates = GenerateCourtCaseDates(courtCases, users);
            await _context.CourtCaseDates.AddRangeAsync(courtCaseDates);
            await _context.SaveChangesAsync();
            Console.WriteLine($"   Created {courtCaseDates.Count} court case dates");

            Console.WriteLine("\n📄 Generating Documents...");
            var documents = GenerateDocuments(courtCases, users);
            await _context.Documents.AddRangeAsync(documents);
            await _context.SaveChangesAsync();
            Console.WriteLine($"   Created {documents.Count} documents");

            Console.WriteLine("\n🧾 Generating Invoices...");
            var invoices = GenerateInvoices(courtCases, users);
            await _context.Invoices.AddRangeAsync(invoices);
            await _context.SaveChangesAsync();
            Console.WriteLine($"   Created {invoices.Count} invoices");

            Console.WriteLine("\n📝 Generating Invoice Items...");
            var invoiceItems = GenerateInvoiceItems(invoices, users);
            await _context.InvoiceItems.AddRangeAsync(invoiceItems);
            await _context.SaveChangesAsync();
            Console.WriteLine($"   Created {invoiceItems.Count} invoice items");

            Console.WriteLine("\n🔗 Associating Lawyers with Cases...");
            AssociateLawyersWithCases(courtCases, lawyers);
            await _context.SaveChangesAsync();
            Console.WriteLine($"   Associated lawyers with cases");

            Console.WriteLine("\n🔗 Associating Lawyers with Case Dates...");
            AssociateLawyersWithCaseDates(courtCaseDates, lawyers);
            await _context.SaveChangesAsync();
            Console.WriteLine($"   Associated lawyers with case dates");
        }

        private List<Firm> GenerateFirms(int count)
        {
            var firmFaker = new Faker<Firm>()
                .RuleFor(f => f.Id, _ => Guid.NewGuid())
                .RuleFor(f => f.Name, f => f.Company.CompanyName() + " Attorneys")
                .RuleFor(f => f.Address, f => f.Address.FullAddress())
                .RuleFor(f => f.Telephone, f => f.Phone.PhoneNumber("+27 ## ### ####"))
                .RuleFor(f => f.Fax, f => f.Phone.PhoneNumber("+27 ## ### ####"))
                .RuleFor(f => f.Mobile, f => f.Phone.PhoneNumber("+27 ## ### ####"))
                .RuleFor(f => f.Email, f => f.Internet.Email())
                .RuleFor(f => f.AttorneyAdmissionDate, f => f.Date.Past(20))
                .RuleFor(f => f.AdvocateAdmissionDate, f => f.Date.Past(15))
                .RuleFor(f => f.AccountName, f => f.Company.CompanyName())
                .RuleFor(f => f.Bank, f => f.PickRandom("Standard Bank", "FNB", "Nedbank", "ABSA", "Capitec"))
                .RuleFor(f => f.BranchCode, f => f.Random.Number(100000, 999999).ToString())
                .RuleFor(f => f.AccountNumber, f => f.Finance.Account(10))
                .RuleFor(f => f.Created, _ => DateTime.UtcNow)
                .RuleFor(f => f.IsDeleted, _ => false);

            return firmFaker.Generate(count);
        }

        private List<User> GenerateUsers(List<Firm> firms, int usersPerFirm)
        {
            var users = new List<User>();

            foreach (var firm in firms)
            {
                var userFaker = new Faker<User>()
                    .RuleFor(u => u.Id, _ => Guid.NewGuid())
                    .RuleFor(u => u.Email, f => f.Internet.Email())
                    .RuleFor(u => u.Name, f => f.Name.FirstName())
                    .RuleFor(u => u.Surname, f => f.Name.LastName())
                    .RuleFor(u => u.MobileNumber, f => f.Phone.PhoneNumber("+27 ## ### ####"))
                    .RuleFor(u => u.PasswordHash, _ => "HASHED_PASSWORD_PLACEHOLDER")
                    .RuleFor(u => u.PasswordSalt, _ => "SALT_PLACEHOLDER")
                    .RuleFor(u => u.Role, f => f.PickRandom<UserRole>())
                    .RuleFor(u => u.FirmId, _ => firm.Id)
                    .RuleFor(u => u.Created, _ => DateTime.UtcNow)
                    .RuleFor(u => u.IsDeleted, _ => false);

                var firmUsers = userFaker.Generate(usersPerFirm);

                // Ensure at least one admin per firm
                firmUsers[0].Role = UserRole.FirmAdmin;

                users.AddRange(firmUsers);
            }

            return users;
        }

        private List<Lawyer> GenerateLawyers(List<User> users)
        {
            var lawyerCount = users.Count / 2; // Generate half as many lawyers as users

            var lawyerFaker = new Faker<Lawyer>()
                .RuleFor(l => l.Id, _ => Guid.NewGuid())
                .RuleFor(l => l.Email, f => f.Internet.Email())
                .RuleFor(l => l.Name, f => f.Name.FirstName())
                .RuleFor(l => l.Surname, f => f.Name.LastName())
                .RuleFor(l => l.MobileNumber, f => f.Phone.PhoneNumber("+27 ## ### ####"))
                .RuleFor(l => l.FirmName, f => f.Company.CompanyName() + " Law Firm")
                .RuleFor(l => l.Specialty, f => f.PickRandom<Speciality>())
                .RuleFor(l => l.CreatedByUserId, f => f.PickRandom(users).Id)
                .RuleFor(l => l.Created, _ => DateTime.UtcNow)
                .RuleFor(l => l.IsDeleted, _ => false);

            return lawyerFaker.Generate(lawyerCount);
        }

        private List<CourtCase> GenerateCourtCases(List<User> users, int casesPerUser)
        {
            var cases = new List<CourtCase>();

            foreach (var user in users)
            {
                var caseFaker = new Faker<CourtCase>()
                    .RuleFor(c => c.Id, _ => Guid.NewGuid())
                    .RuleFor(c => c.CaseNumber, f => $"CC-{f.Random.Number(1000, 9999)}/{DateTime.Now.Year}")
                    .RuleFor(c => c.Location, f => f.Address.City() + " High Court")
                    .RuleFor(c => c.Plaintiff, f => f.Name.FullName())
                    .RuleFor(c => c.Defendant, f => f.Name.FullName())
                    .RuleFor(c => c.Status, f => f.PickRandom<CourtCaseStatus>())
                    .RuleFor(c => c.Type, (CourtCaseTypes)_random.Next(0, 13))
                    .RuleFor(c => c.Outcome, (CourtCaseOutcomes)_random.Next(0, 14))
                    .RuleFor(c => c.IsPaid, f => f.Random.Bool(0.7f))
                    .RuleFor(c => c.UserId, _ => user.Id)
                    .RuleFor(c => c.Created, _ => DateTime.UtcNow.AddDays(-_random.Next(1, 365)))
                    .RuleFor(c => c.IsDeleted, _ => false);

                var userCases = caseFaker.Generate(_random.Next(1, casesPerUser + 1));
                cases.AddRange(userCases);
            }

            return cases;
        }

        private List<CourtCaseDate> GenerateCourtCaseDates(List<CourtCase> courtCases, List<User> users)
{
    var dates = new List<CourtCaseDate>();

    // Define realistic court case date scenarios
    var courtDateScenarios = new[]
    {
        new {
            Title = "Pre-Trial Hearing",
            Type = "Court Appearance",
            Description = "A preliminary court session to address procedural matters, resolve pending motions, and establish trial readiness before the main trial begins."
        },
        new {
            Title = "Case Management Conference",
            Type = "Court Appearance",
            Description = "A meeting with the judge to discuss case progress, set deadlines, and manage the litigation timeline to ensure efficient case resolution."
        },
        new {
            Title = "Motion Hearing",
            Type = "Court Appearance",
            Description = "A court session where the judge hears arguments on specific motions filed by either party and makes rulings on legal issues."
        },
        new {
            Title = "Settlement Conference",
            Type = "Settlement Discussion",
            Description = "A facilitated negotiation session aimed at resolving the dispute without trial, often involving settlement offers and counteroffers."
        },
        new {
            Title = "Trial Date",
            Type = "Court Appearance",
            Description = "The scheduled date for the full trial where evidence is presented, witnesses testify, and the case is decided by a judge or jury."
        },
        new {
            Title = "Sentencing Hearing",
            Type = "Court Appearance",
            Description = "A court session following a conviction where the judge determines and imposes the appropriate punishment or sentence."
        },
        new {
            Title = "Bail Application",
            Type = "Court Appearance",
            Description = "A hearing to request the release of a defendant from custody pending trial, with conditions set to ensure court appearance."
        },
        new {
            Title = "Discovery Deadline",
            Type = "Filing Deadline",
            Description = "The final date by which all parties must complete the exchange of relevant documents, evidence, and information."
        },
        new {
            Title = "Expert Witness Deadline",
            Type = "Filing Deadline",
            Description = "The cutoff date for designating expert witnesses and submitting their reports to opposing counsel and the court."
        },
        new {
            Title = "Client Consultation",
            Type = "Consultation",
            Description = "A scheduled meeting with the client to discuss case strategy, provide updates, gather information, and address concerns."
        },
        new {
            Title = "Witness Interview",
            Type = "Consultation",
            Description = "A meeting to question and prepare witnesses who may provide testimony or relevant information for the case."
        },
        new {
            Title = "Mediation Session",
            Type = "Settlement Discussion",
            Description = "A structured negotiation process facilitated by a neutral third-party mediator to help parties reach a mutually acceptable resolution."
        },
        new {
            Title = "Arbitration Hearing",
            Type = "Court Appearance",
            Description = "A formal hearing before an arbitrator who will hear evidence and arguments to render a binding decision on the dispute."
        },
        new {
            Title = "Status Conference",
            Type = "Court Appearance",
            Description = "A brief court session to update the judge on case progress, discuss any issues, and confirm upcoming deadlines and hearings."
        },
        new {
            Title = "Plea Hearing",
            Type = "Court Appearance",
            Description = "A court proceeding where the defendant formally enters a plea of guilty, not guilty, or no contest to criminal charges."
        },
        new {
            Title = "Appeal Hearing",
            Type = "Court Appearance",
            Description = "A hearing before an appellate court to review legal errors from a lower court's decision and determine if reversal is warranted."
        },
        new {
            Title = "Motion to Dismiss Hearing",
            Type = "Court Appearance",
            Description = "A hearing where one party argues that the case should be dismissed due to legal deficiencies or lack of valid claims."
        },
        new {
            Title = "Discovery Conference",
            Type = "Discovery",
            Description = "A meeting to discuss and resolve disputes related to the discovery process, including document requests and depositions."
        },
        new {
            Title = "Document Production Deadline",
            Type = "Filing Deadline",
            Description = "The final date by which requested documents and evidence must be provided to the opposing party in compliance with discovery orders."
        },
        new {
            Title = "Interrogatory Responses Due",
            Type = "Filing Deadline",
            Description = "The deadline for submitting written answers to formal written questions (interrogatories) posed by the opposing party."
        }
    };

    foreach (var courtCase in courtCases)
    {
        var dateFaker = new Faker<CourtCaseDate>()
            .CustomInstantiator(f =>
            {
                var scenario = f.PickRandom(courtDateScenarios);
                return new CourtCaseDate
                {
                    Id = Guid.NewGuid(),
                    Date = f.Date.Future(1).ToString("yyyy-MM-dd HH:mm"),
                    Title = scenario.Title,
                    Description = scenario.Description,
                    Type = (CourtCaseDateTypes)_random.Next(1, 14),
                    CaseId = courtCase.Id,
                    UserId = courtCase.UserId,
                    Created = DateTime.UtcNow,
                    IsDeleted = false,
                    IsComplete = false,
                    IsCanceled =  false,
                };
            });

        var caseDates = dateFaker.Generate(_random.Next(1, 5));
        dates.AddRange(caseDates);
    }

    return dates;
}

        private List<Document> GenerateDocuments(List<CourtCase> courtCases, List<User> users)
        {
            var documents = new List<Document>();

            foreach (var courtCase in courtCases)
            {
                var docFaker = new Faker<Document>()
                    .RuleFor(d => d.Id, _ => Guid.NewGuid())
                    .RuleFor(d => d.Name, f => f.PickRandom(
                        "Complaint", "Answer", "Motion to Dismiss", "Discovery Request",
                        "Witness Statement", "Expert Report", "Settlement Agreement"))
                    .RuleFor(d => d.FileName, (f, d) => $"{d.Name.Replace(" ", "_")}_{f.Random.Number(100, 999)}.pdf")
                    .RuleFor(d => d.ContentType, _ => "application/pdf")
                    .RuleFor(d => d.Size, f => f.Random.Long(10240, 5242880)) // 10KB to 5MB
                    .RuleFor(d => d.UserId, _ => courtCase.UserId)
                    .RuleFor(d => d.CaseId, _ => courtCase.Id)
                    .RuleFor(d => d.Created, _ => DateTime.UtcNow)
                    .RuleFor(d => d.IsDeleted, _ => false);

                var caseDocuments = docFaker.Generate(_random.Next(2, 8));
                documents.AddRange(caseDocuments);
            }

            return documents;
        }

        private List<Invoice> GenerateInvoices(List<CourtCase> courtCases, List<User> users)
        {
            var invoices = new List<Invoice>();
            var invoiceNumber = 1000;

            foreach (var courtCase in courtCases)
            {
                // Not all cases have invoices
                if (_random.Next(0, 100) < 80) // 80% chance of having an invoice
                {
                    var invFaker = new Faker<Invoice>()
                        .RuleFor(i => i.Id, _ => Guid.NewGuid())
                        .RuleFor(i => i.InvoiceNumber, _ => $"INV-{invoiceNumber++}")
                        .RuleFor(i => i.InvoiceDate, f => f.Date.Recent(90))
                        .RuleFor(i => i.ClientName, _ => courtCase.Plaintiff)
                        .RuleFor(i => i.Reference, f => f.Random.Bool(0.5f) ? $"REF-{f.Random.Number(1000, 9999)}" : null)
                        .RuleFor(i => i.AccountName, f => f.Company.CompanyName())
                        .RuleFor(i => i.Bank, f => f.PickRandom("Standard Bank", "FNB", "Nedbank", "ABSA"))
                        .RuleFor(i => i.BranchCode, f => f.Random.Number(100000, 999999).ToString())
                        .RuleFor(i => i.AccountNumber, f => f.Finance.Account(10))
                        .RuleFor(i => i.UserId, _ => courtCase.UserId)
                        .RuleFor(i => i.CaseId, _ => courtCase.Id)
                        .RuleFor(i => i.Status, f => f.PickRandom<InvoiceStatus>())
                        .RuleFor(i => i.Created, _ => DateTime.UtcNow)
                        .RuleFor(i => i.IsDeleted, _ => false);

                    invoices.Add(invFaker.Generate());
                }
            }

            return invoices;
        }

        private List<InvoiceItem> GenerateInvoiceItems(List<Invoice> invoices, List<User> users)
        {
            var items = new List<InvoiceItem>();

            foreach (var invoice in invoices)
            {
                var itemFaker = new Faker<InvoiceItem>()
                    .RuleFor(i => i.Id, _ => Guid.NewGuid())
                    .RuleFor(i => i.InvoiceId, _ => invoice.Id)
                    .RuleFor(i => i.Name, f => f.PickRandom(
                        "Legal Consultation", "Court Appearance", "Document Preparation",
                        "Research", "Travel Time", "Administrative Work", "Expert Witness Fee"))
                    .RuleFor(i => i.Hours, f => f.Random.Number(1, 20))
                    .RuleFor(i => i.CostPerHour, f => f.Finance.Amount(500, 5000))
                    .RuleFor(i => i.UserId, _ => invoice.UserId)
                    .RuleFor(i => i.Created, _ => DateTime.UtcNow)
                    .RuleFor(i => i.IsDeleted, _ => false);

                var invoiceItems = itemFaker.Generate(_random.Next(2, 6));
                items.AddRange(invoiceItems);
            }

            return items;
        }

        private void AssociateLawyersWithCases(List<CourtCase> courtCases, List<Lawyer> lawyers)
        {
            foreach (var courtCase in courtCases)
            {
                // Random chance of having external lawyers
                if (_random.Next(0, 100) < 60) // 60% chance
                {
                    var randomLawyers = lawyers
                        .OrderBy(_ => _random.Next())
                        .Take(_random.Next(1, 4))
                        .ToList();

                    foreach (var lawyer in randomLawyers)
                    {
                        if (!courtCase.Lawyers.Contains(lawyer))
                        {
                            courtCase.Lawyers.Add(lawyer);
                        }
                    }
                }
            }
        }

        private void AssociateLawyersWithCaseDates(List<CourtCaseDate> courtCaseDates, List<Lawyer> lawyers)
        {
            foreach (var caseDate in courtCaseDates)
            {
                // Random chance of having lawyers at specific dates
                if (_random.Next(0, 100) < 40) // 40% chance
                {
                    var randomLawyers = lawyers
                        .OrderBy(_ => _random.Next())
                        .Take(_random.Next(1, 3))
                        .ToList();

                    foreach (var lawyer in randomLawyers)
                    {
                        if (!caseDate.Lawyers.Contains(lawyer))
                        {
                            caseDate.Lawyers.Add(lawyer);
                        }
                    }
                }
            }
        }
    }
}
