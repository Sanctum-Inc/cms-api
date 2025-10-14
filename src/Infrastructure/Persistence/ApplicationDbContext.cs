using Domain.CourtCaseDates;
using Domain.CourtCases;
using Domain.Documents;
using Domain.InvoiceItems;
using Domain.Lawyers;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence
{
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

            // Unique indexes
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Lawyer>()
                .HasIndex(l => l.Email)
                .IsUnique();

            modelBuilder.Entity<CourtCase>()
                .HasIndex(c => c.CaseNumber)
                .IsUnique();

            // Many-to-Many: CourtCase <-> Lawyer
            modelBuilder.Entity<CourtCase>()
                .HasMany(c => c.Lawyers)
                .WithMany(l => l.CourtCases)
                .UsingEntity<Dictionary<string, object>>(
                    "CourtCaseLawyers", // Table name
                    j => j.HasOne<Lawyer>().WithMany().HasForeignKey("LawyerId"),
                    j => j.HasOne<CourtCase>().WithMany().HasForeignKey("CourtCaseId"));

            // Many-to-Many: CourtCaseDate <-> Lawyer
            modelBuilder.Entity<CourtCaseDate>()
                .HasMany(d => d.Lawyers)
                .WithMany(l => l.CourtCaseDates)
                .UsingEntity<Dictionary<string, object>>(
                    "CourtCaseDateLawyers", // Table name
                    j => j.HasOne<Lawyer>().WithMany().HasForeignKey("LawyerId"),
                    j => j.HasOne<CourtCaseDate>().WithMany().HasForeignKey("CourtCaseDateId"));
        }
    }
}