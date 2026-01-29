using Domain.CourtCases;
using Domain.Lawyers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class CourtCaseConfiguration : IEntityTypeConfiguration<CourtCase>
{
    public void Configure(EntityTypeBuilder<CourtCase> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.CaseNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.Location).IsRequired();
        builder.Property(c => c.Plaintiff).IsRequired();
        builder.Property(c => c.Defendant).IsRequired();
        builder.Property(c => c.Status).IsRequired();
        builder.Property(c => c.UserId).IsRequired();

        // CourtCase -> User
        builder.HasOne(c => c.User)
            .WithMany(u => u.CourtCases)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        // Many-to-Many: CourtCase <-> Lawyer
        builder.HasMany(c => c.Lawyers)
            .WithMany(l => l.CourtCases)
            .UsingEntity<Dictionary<string, object>>(
                "CourtCaseLawyers",
                j => j.HasOne<Lawyer>()
                      .WithMany()
                      .HasForeignKey("LawyerId")
                      .OnDelete(DeleteBehavior.NoAction),

                j => j.HasOne<CourtCase>()
                      .WithMany()
                      .HasForeignKey("CourtCaseId")
                      .OnDelete(DeleteBehavior.Cascade)
            );

        // CourtCase -> CourtCaseDates
        builder.HasMany(c => c.CourtCaseDates)
            .WithOne()
            .HasForeignKey(d => d.CaseId)
            .OnDelete(DeleteBehavior.Cascade);

        // CourtCase -> Documents
        builder.HasMany(c => c.Documents)
            .WithOne(d => d.Case)
            .HasForeignKey(d => d.CaseId)
            .OnDelete(DeleteBehavior.Cascade);

        // CourtCase -> Invoices
        builder.HasMany(c => c.Invoices)
            .WithOne()
            .HasForeignKey(i => i.CaseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
