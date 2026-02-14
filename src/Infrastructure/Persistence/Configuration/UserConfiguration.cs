using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserConfiguration : BaseConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        // IMPORTANT: Call base configuration FIRST
        base.Configure(builder);

        // Then add entity-specific configuration
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Surname)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.MobileNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(u => u.PasswordHash)
            .IsRequired();

        builder.Property(u => u.PasswordSalt)
            .IsRequired();

        builder.Property(u => u.Role)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(u => u.VerificationEmailSentCount)
            .HasDefaultValue(0);

        builder.Property(u => u.EmailVerificationTokenVersion)
            .HasDefaultValue(1);

        builder.Property(u => u.IsEmailVerified)
            .IsRequired()
            .HasDefaultValue(false);

        // Relationship with Firm - RESTRICT
        builder
            .HasOne(u => u.Firm)
            .WithMany(f => f.Users)
            .HasForeignKey(u => u.FirmId)
            .OnDelete(DeleteBehavior.Restrict); // Don't cascade delete users when firm is deleted
    }
}
