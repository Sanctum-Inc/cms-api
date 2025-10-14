using Domain.CourtCaseDates;
using Domain.CourtCases;
using Domain.Lawyers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;
public class LawyerConfiguration : IEntityTypeConfiguration<Domain.Lawyers.Lawyer>
{
    public void Configure(EntityTypeBuilder<Lawyer> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Name)
            .IsRequired();

        builder.Property(l => l.Surname)
            .IsRequired();

        builder.Property(l => l.Email)
            .IsRequired();

        builder.Property(l => l.MobileNumber)
            .IsRequired();

        // Many-to-Many: Lawyer <-> CourtCase
        builder.HasMany(l => l.CourtCases)
            .WithMany(c => c.Lawyers)
            .UsingEntity<Dictionary<string, object>>(
                "CourtCaseLawyers", // Table name
                j => j.HasOne<CourtCase>().WithMany().HasForeignKey("CourtCaseId"),
                j => j.HasOne<Lawyer>().WithMany().HasForeignKey("LawyerId"));

        builder.HasMany(l => l.CourtCaseDates)
            .WithMany(d => d.Lawyers)
            .UsingEntity<Dictionary<string, object>>(
                "LawyerCourtCaseDates", // Table name
                j => j.HasOne<CourtCaseDate>().WithMany().HasForeignKey("CourtCaseDateId"),
                j => j.HasOne<Lawyer>().WithMany().HasForeignKey("LawyerId"));
    }
}