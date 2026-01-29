using Domain.InvoiceItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class InvoiceItemConfiguration : IEntityTypeConfiguration<InvoiceItem>
{
    public void Configure(EntityTypeBuilder<InvoiceItem> builder)
    {
        builder.HasKey(ii => ii.Id);

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

        // Relationship with User - NO ACTION (to prevent cascade conflicts)
        builder
            .HasOne(ii => ii.User)
            .WithMany()
            .HasForeignKey(ii => ii.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
