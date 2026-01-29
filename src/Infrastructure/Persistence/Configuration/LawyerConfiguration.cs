using Domain.Lawyers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class LawyerConfiguration : IEntityTypeConfiguration<Lawyer>
{
    public void Configure(EntityTypeBuilder<Lawyer> builder)
    {
        builder.HasKey(l => l.Id);

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

        // Relationship with User (CreatedBy) - NO ACTION (to prevent cascade conflicts)
        builder
            .HasOne(l => l.CreatedByUser)
            .WithMany(u => u.Lawyers)
            .HasForeignKey(l => l.CreatedByUserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
