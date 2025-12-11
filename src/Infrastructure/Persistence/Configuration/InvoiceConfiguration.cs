using Domain.Invoices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

internal class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.InvoiceNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(i => i.ClientName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(i => i.Reference)
            .HasMaxLength(100);

        builder.Property(i => i.AccountName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(i => i.Bank)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.BranchCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(i => i.AccountNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder
            .HasOne(i => i.User)
            .WithMany(u => u.Invoices)
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(i => i.Case)
            .WithMany(c => c.Invoices)
            .HasForeignKey(i => i.CaseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(i => i.Items)
            .WithOne(ii => ii.Invoice)
            .HasForeignKey(ii => ii.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
