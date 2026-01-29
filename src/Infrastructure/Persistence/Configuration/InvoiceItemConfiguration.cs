using Domain.InvoiceItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

internal class InvoiceItemConfiguration : IEntityTypeConfiguration<InvoiceItem>
{
    public void Configure(EntityTypeBuilder<InvoiceItem> builder)
    {
        builder.HasKey(ii => ii.Id);

        builder.Property(ii => ii.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(ii => ii.Hours)
            .HasDefaultValue(0);

        builder.Property(ii => ii.CostPerHour)
            .HasColumnType("decimal(18,2)");

        builder
            .HasOne(ii => ii.Invoice)
            .WithMany(i => i.Items)
            .HasForeignKey(ii => ii.InvoiceId);
    }
}
