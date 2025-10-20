using Domain.Invoices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.InvoiceNumber)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(i => i.InvoiceDate)
                .IsRequired();

            builder.Property(i => i.ClientName)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(i => i.Reference)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(i => i.CaseName)
                .HasMaxLength(300)
                .IsRequired();

            builder.Property(i => i.TotalAmount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(i => i.AccountName)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(i => i.Bank)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(i => i.BranchCode)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(i => i.AccountNumber)
                .HasMaxLength(50)
                .IsRequired();

            // One-to-Many: Invoice -> InvoiceItems
            builder.HasMany(i => i.Items)
                .WithOne(ii => ii.Invoice)
                .HasForeignKey(ii => ii.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
