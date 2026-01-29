using Domain.Lawyers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class LawyerConfiguration : BaseConfiguration<Lawyer>
{
    public override void Configure(EntityTypeBuilder<Lawyer> builder)
    {
        // IMPORTANT: Call base configuration FIRST
        base.Configure(builder);

        // Then add entity-specific configuration
        builder.Property(l => l.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(l => l.Email);

        builder.Property(l => l.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(l => l.Surname)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(l => l.MobileNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(l => l.FirmName)
            .HasMaxLength(200);

        builder.Property(l => l.Specialty)
            .IsRequired()
            .HasConversion<int>();

        // CRITICAL: Relationship with User (CreatedBy) - NO ACTION (to prevent cascade conflicts)
        builder
            .HasOne(l => l.CreatedByUser)
            .WithMany(u => u.Lawyers)
            .HasForeignKey(l => l.CreatedByUserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
