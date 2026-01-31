using Domain.CourtCases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class CourtCaseConfiguration : BaseConfiguration<CourtCase>
{
    public override void Configure(EntityTypeBuilder<CourtCase> builder)
    {
        // IMPORTANT: Call base configuration FIRST
        base.Configure(builder);

        // Then add entity-specific configuration
        builder.Property(c => c.CaseNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(c => c.CaseNumber)
            .IsUnique();

        builder.Property(c => c.Location)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Plaintiff)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Defendant)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(c => c.Type)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Outcome)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(c => c.IsPaid)
            .IsRequired();

        // CRITICAL: Relationship with User - NO ACTION to prevent cascade conflicts
        builder
            .HasOne(c => c.User)
            .WithMany(u => u.CourtCases)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        // Many-to-Many with Lawyers
        builder
            .HasMany(c => c.Lawyers)
            .WithMany(l => l.CourtCases)
            .UsingEntity(j => j.ToTable("CourtCaseLawyers"));
    }
}
