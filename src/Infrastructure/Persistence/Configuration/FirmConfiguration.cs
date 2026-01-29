using Domain.Firms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class FirmConfiguration : BaseConfiguration<Firm>
{
    public override void Configure(EntityTypeBuilder<Firm> builder)
    {
        // IMPORTANT: Call base configuration FIRST
        base.Configure(builder);

        // Then add entity-specific configuration
        builder.Property(f => f.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(f => f.Address)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(f => f.Telephone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(f => f.Fax)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(f => f.Mobile)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(f => f.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(f => f.Email)
            .IsUnique();

        builder.Property(f => f.AttorneyAdmissionDate)
            .IsRequired();

        builder.Property(f => f.AdvocateAdmissionDate)
            .IsRequired();

        builder.Property(f => f.AccountName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(f => f.Bank)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(f => f.BranchCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(f => f.AccountNumber)
            .IsRequired()
            .HasMaxLength(50);
    }
}
