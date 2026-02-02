using System.Reflection;
using Application.Common.Interfaces.Persistence;
using Domain.Common;
using Domain.CourtCaseDates;
using Domain.CourtCases;
using Domain.Documents;
using Domain.Firms;
using Domain.InvoiceItems;
using Domain.Invoices;
using Domain.Lawyers;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Persistence;

public class ApplicationDBContext : DbContext, IApplicationDBContext
{
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
        : base(options)
    {
    }

    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<Firm> Firms => Set<Firm>();

    public DbSet<User> Users => Set<User>();
    public DbSet<CourtCase> CourtCases => Set<CourtCase>();
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<InvoiceItem> InvoiceItems => Set<InvoiceItem>();
    public DbSet<Lawyer> Lawyers => Set<Lawyer>();
    public DbSet<CourtCaseDate> CourtCaseDates => Set<CourtCaseDate>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // CRITICAL: Call base.OnModelCreating FIRST
        base.OnModelCreating(modelBuilder);

        // CRITICAL: Apply all entity configurations from assembly
        // This is what was missing!
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Set up soft delete query filters AFTER configurations are applied
        foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                     .Where(t => typeof(AuditableEntity).IsAssignableFrom(t.ClrType)))
        {
            var method = typeof(ApplicationDBContext)
                .GetMethod(nameof(SetGlobalQueryFilter), BindingFlags.NonPublic | BindingFlags.Static)!
                .MakeGenericMethod(entityType.ClrType);

            method.Invoke(null, new object[] { modelBuilder });
        }

        // OPTIONAL BUT RECOMMENDED: Override critical cascade behaviors inline
        // This ensures they're set correctly even if configurations have issues
        ConfigureCascadeBehaviors(modelBuilder);
    }

    private static void ConfigureCascadeBehaviors(ModelBuilder modelBuilder)
    {
        // User -> Firm: RESTRICT (prevent firm deletion if users exist)
        modelBuilder.Entity<User>()
            .HasOne(u => u.Firm)
            .WithMany(f => f.Users)
            .HasForeignKey(u => u.FirmId)
            .OnDelete(DeleteBehavior.Restrict);

        // CourtCase -> User: NO ACTION (prevent cascade conflicts)
        modelBuilder.Entity<CourtCase>()
            .HasOne(c => c.User)
            .WithMany(u => u.CourtCases)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        // CourtCaseDate -> User: NO ACTION (prevent cascade conflicts)
        modelBuilder.Entity<CourtCaseDate>()
            .HasOne(d => d.User)
            .WithMany(u => u.CourtCasesDates)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        // Document -> User: NO ACTION (prevent cascade conflicts)
        modelBuilder.Entity<Document>()
            .HasOne(d => d.User)
            .WithMany(u => u.Documents)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        // Invoice -> User: NO ACTION (prevent cascade conflicts)
        modelBuilder.Entity<Invoice>()
            .HasOne(i => i.User)
            .WithMany(u => u.Invoices)
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        // InvoiceItem -> User: NO ACTION (prevent cascade conflicts)
        modelBuilder.Entity<InvoiceItem>()
            .HasOne(ii => ii.User)
            .WithMany()
            .HasForeignKey(ii => ii.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        // Lawyer -> User (CreatedBy): NO ACTION (prevent cascade conflicts)
        modelBuilder.Entity<Lawyer>()
            .HasOne(l => l.CreatedByUser)
            .WithMany(u => u.Lawyers)
            .HasForeignKey(l => l.CreatedByUserId)
            .OnDelete(DeleteBehavior.NoAction);
    }

    private static void SetGlobalQueryFilter<TEntity>(ModelBuilder builder) where TEntity : AuditableEntity
    {
        builder.Entity<TEntity>().HasQueryFilter(e => !e.IsDeleted);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .ConfigureWarnings(warnings =>
                warnings.Ignore(CoreEventId.AccidentalEntityType));
    }
}
