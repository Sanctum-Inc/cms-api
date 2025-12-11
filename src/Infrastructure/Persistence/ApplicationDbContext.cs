using System.Reflection;
using Application.Common.Interfaces.Persistence;
using Domain.Common;
using Domain.CourtCaseDates;
using Domain.Documents;
using Domain.InvoiceItems;
using Domain.Invoices;
using Domain.Lawyers;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Linq;
using Domain.Firms;

namespace Infrastructure.Persistence;

public class ApplicationDBContext : DbContext, IApplicationDBContext
{
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Domain.CourtCases.CourtCase> CourtCases => Set<Domain.CourtCases.CourtCase>();
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceItem> InvoiceItems => Set<InvoiceItem>();
    public DbSet<Lawyer> Lawyers => Set<Lawyer>();
    public DbSet<CourtCaseDate> CourtCaseDates => Set<CourtCaseDate>();
    public DbSet<Firm> Firms => Set<Firm>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var method in from entityType in modelBuilder.Model.GetEntityTypes()
                               where typeof(AuditableEntity).IsAssignableFrom(entityType.ClrType)
                               let method = typeof(ApplicationDBContext)
                            .GetMethod(nameof(SetGlobalQueryFilter), BindingFlags.NonPublic | BindingFlags.Static)!
                            .MakeGenericMethod(entityType.ClrType)
                               select method)
        {
            method.Invoke(null, [modelBuilder]);
        }

        base.OnModelCreating(modelBuilder);
    }

    private static void SetGlobalQueryFilter<TEntity>(ModelBuilder builder) where TEntity : AuditableEntity
    {
        builder.Entity<TEntity>().HasQueryFilter(e => !e.IsDeleted);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            // your existing options
            .ConfigureWarnings(warnings =>
                warnings.Ignore(CoreEventId.AccidentalEntityType));
    }

}
