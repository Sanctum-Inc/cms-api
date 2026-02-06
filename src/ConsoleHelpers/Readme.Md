# Court Management System - Seed Data Generator

A console application to generate realistic seed data for the Court Management System using the Bogus library.

## Features

- 🏢 Generates law firms with complete details
- 👤 Creates users (admin and regular) for each firm
- ⚖️ Generates external lawyers from other firms
- 📋 Creates court cases with realistic data
- 📅 Adds court case dates/appointments
- 📄 Generates documents for each case
- 🧾 Creates invoices and invoice items
- 🔗 Associates lawyers with cases and dates

## Setup

### 1. Create the Project

```bash
# Create a new console project in your solution
cd your-solution-directory
mkdir SeedDataGenerator
cd SeedDataGenerator

# Copy the files
# - Program.cs
# - SeedDataGenerator.csproj
# - appsettings.json

# Add to your solution
dotnet sln add SeedDataGenerator/SeedDataGenerator.csproj
```

### 2. Update appsettings.json

Edit `appsettings.json` to match your database connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=YOUR_DATABASE;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

### 3. Update Project References

In `SeedDataGenerator.csproj`, update the project references to match your project structure:

```xml
<ProjectReference Include="..\Domain\Domain.csproj" />
<ProjectReference Include="..\Infrastructure\Infrastructure.csproj" />
```

### 4. Install Dependencies

```bash
dotnet restore
```

## Usage

### Run the Generator

```bash
dotnet run
```

### Interactive Prompts

The program will ask you:

1. **How many firms?** (default: 5)
    - Each firm represents a law firm

2. **How many users per firm?** (default: 10)
    - Mix of admin and regular users
    - At least one admin per firm is guaranteed

3. **How many court cases per user?** (default: 5)
    - Each user gets 1 to N cases (random)

### Example Session

```
=== Court Management System - Seed Data Generator ===

How many firms do you want to generate? (default: 5): 3
How many users per firm? (default: 10): 8
How many court cases per user? (default: 5): 4

Generating:
  - 3 firms
  - 24 users (8 per firm)
  - ~96 court cases

Proceed? (y/n): y

Ensuring database exists...

🏢 Generating Firms...
   Created 3 firms

👤 Generating Users...
   Created 24 users

⚖️ Generating External Lawyers...
   Created 12 external lawyers

📋 Generating Court Cases...
   Created 89 court cases

📅 Generating Court Case Dates...
   Created 234 court case dates

📄 Generating Documents...
   Created 412 documents

🧾 Generating Invoices...
   Created 71 invoices

📝 Generating Invoice Items...
   Created 283 invoice items

🔗 Associating Lawyers with Cases...
   Associated lawyers with cases

🔗 Associating Lawyers with Case Dates...
   Associated lawyers with case dates

✅ Seed data generation complete!
```

## Generated Data Details

### Firms
- Realistic South African law firm names
- Complete contact details (phone, fax, mobile, email)
- Banking information
- Admission dates for attorneys and advocates

### Users
- Realistic names and contact information
- Email addresses
- Mobile numbers in SA format (+27)
- Role assignment (Admin/Regular)
- Password hash/salt placeholders (you'll need to update these)

### Court Cases
- Unique case numbers (CC-XXXX/YEAR)
- Various case types (Civil, Criminal, Family, Commercial, Labor)
- Court locations (SA High Courts)
- Plaintiff and defendant names
- Case status (Open, InProgress, Closed, Archived)
- Payment status
- Outcomes (30% of cases)

### Court Case Dates
- Future appointment dates
- Various types (Hearing, Trial, Motion, Conference)
- Associated with cases and users

### Documents
- Realistic legal document names
- PDF file format
- File sizes (10KB - 5MB)
- Associated with cases

### Invoices
- Sequential invoice numbers (INV-1000, INV-1001, etc.)
- Recent invoice dates (last 90 days)
- Client names (from plaintiffs)
- Banking details
- Status (Pending, Paid, Overdue, Cancelled)

### Invoice Items
- Legal service descriptions
- Hourly rates (R500 - R5000)
- Hours worked (1-20 hours)
- Associated with users who performed work

### Lawyers
- External lawyers from other firms
- Contact information
- Specialties (Criminal, Civil, Family, etc.)
- Associated with cases (60% of cases have external lawyers)
- Associated with case dates (40% of dates have lawyers)

## Data Relationships

The generator ensures:

✅ All foreign key relationships are valid
✅ No orphaned records
✅ Realistic associations between entities
✅ At least one admin per firm
✅ Proper audit trail (Created timestamps)
✅ Soft delete flags set correctly

## Customization

### Adjust Randomization

Edit the probabilities in `SeedDataGenerator` class:

```csharp
// In GenerateInvoices
if (_random.Next(0, 100) < 80) // 80% of cases have invoices

// In AssociateLawyersWithCases
if (_random.Next(0, 100) < 60) // 60% of cases have external lawyers
```

### Add More Data Types

Add new faker rules:

```csharp
var customFaker = new Faker<YourEntity>()
    .RuleFor(e => e.Property, f => f.Lorem.Sentence());
```

### Modify Data Ranges

Change the ranges for random data:

```csharp
.RuleFor(c => c.CostPerHour, f => f.Finance.Amount(500, 5000)) // Change min/max
.RuleFor(i => i.Hours, f => f.Random.Number(1, 20)) // Change hours range
```

## Notes

⚠️ **Important:** The generated passwords are placeholders. You'll need to:
1. Run this generator
2. Update password hashes with proper bcrypt/argon2 hashes
3. Or modify the generator to create proper hashes

⚠️ **Database:** This uses `EnsureCreatedAsync()` which works for development but you should use migrations in production.

⚠️ **Performance:** Generating large amounts of data (1000+ firms) may take several minutes.

## Troubleshooting

### "Database connection failed"
- Check your connection string in `appsettings.json`
- Ensure SQL Server is running
- Verify database exists or app has permission to create it

### "Foreign key constraint errors"
- Ensure your database migrations are up to date
- Check that cascade delete behaviors are configured correctly

### "Out of memory"
- Reduce the number of records to generate
- The generator processes in batches with SaveChanges calls

## License

Part of the Court Management System project.
