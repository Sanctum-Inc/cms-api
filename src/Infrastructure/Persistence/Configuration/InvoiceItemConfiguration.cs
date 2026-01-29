using Domain.InvoiceItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class InvoiceItemConfiguration : BaseConfiguration<InvoiceItem>
{
    public override void Configure(EntityTypeBuilder<InvoiceItem> builder)
    {
        // IMPORTANT: Call base configuration FIRST
        base.Configure(builder);

        // Then add entity-specific configuration
        builder.Property(ii => ii.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(ii => ii.Hours)
            .IsRequired();

        builder.Property(ii => ii.CostPerHour)
            .IsRequired()
            .HasPrecision(18, 2);

        // Relationship with Invoice - CASCADE (primary relationship)
        builder
            .HasOne(ii => ii.Invoice)
            .WithMany(i => i.Items)
            .HasForeignKey(ii => ii.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        // CRITICAL: Relationship with User - NO ACTION (to prevent cascade conflicts)
        builder
            .HasOne(ii => ii.User)
            .WithMany()
            .HasForeignKey(ii => ii.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
