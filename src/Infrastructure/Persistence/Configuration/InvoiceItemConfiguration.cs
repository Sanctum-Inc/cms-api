using Domain.InvoiceItems;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Configuration;
public class InvoiceItemConfiguration : IEntityTypeConfiguration<InvoiceItem>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<InvoiceItem> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.InvoiceNumber)
            .IsRequired();

        builder.Property(i => i.CostPerHour)
            .IsRequired()
            .HasPrecision(2);

        builder.Property(i => i.Name)
            .IsRequired();

        builder.Property(i => i.Hours)
            .IsRequired();

        builder.Property(i => i.UserId)
            .IsRequired();

        builder.Property(i => i.CaseId)
            .IsRequired();

        builder.HasOne(i => i.Case)
            .WithMany(c => c.InvoiceItems)
            .HasForeignKey(i => i.CaseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(i => i.User)
            .WithMany(u => u.InvoiceItems)
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}