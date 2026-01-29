using Domain.CourtCaseDates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class CourtCaseDateConfiguration : BaseConfiguration<CourtCaseDate>
{
    public override void Configure(EntityTypeBuilder<CourtCaseDate> builder)
    {
        // IMPORTANT: Call base configuration FIRST
        base.Configure(builder);

        // Then add entity-specific configuration
        builder.Property(d => d.Date)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(d => d.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(d => d.Type)
            .IsRequired()
            .HasMaxLength(100);

        // CRITICAL: Configure relationships AFTER base configuration

        // Relationship with CourtCase - CASCADE (primary relationship)
        builder
            .HasOne(ccd => ccd.Case)
            .WithMany(c => c.CourtCaseDates)
            .HasForeignKey(ccd => ccd.CaseId)
            .OnDelete(DeleteBehavior.Cascade);

        // CRITICAL: Relationship with User - NO ACTION (to prevent cascade conflicts)
        builder
            .HasOne(ccd => ccd.User)
            .WithMany(u => u.CourtCasesDates)
            .HasForeignKey(ccd => ccd.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        // Many-to-Many with Lawyers
        builder
            .HasMany(ccd => ccd.Lawyers)
            .WithMany(l => l.CourtCaseDates)
            .UsingEntity(j => j.ToTable("CourtCaseDateLawyers"));
    }
}
