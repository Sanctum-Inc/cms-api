using Domain.CourtCaseDates;
using Domain.Documents;
using Domain.InvoiceItems;
using Domain.Lawyers;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<CourtCase> CourtCases => Set<CourtCase>();
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<InvoiceItem> InvoiceItems => Set<InvoiceItem>();
    public DbSet<Lawyer> Lawyers => Set<Lawyer>();
    public DbSet<CourtCaseDate> CourtCaseDates => Set<CourtCaseDate>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            // your existing options
            .ConfigureWarnings(warnings =>
                warnings.Ignore(CoreEventId.AccidentalEntityType));
    }
}