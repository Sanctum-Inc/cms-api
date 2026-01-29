using Domain.Invoices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.InvoiceNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(i => i.InvoiceNumber)
            .IsUnique();

        builder.Property(i => i.InvoiceDate)
            .IsRequired();

        builder.Property(i => i.ClientName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(i => i.Reference)
            .HasMaxLength(100);

        builder.Property(i => i.AccountName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(i => i.Bank)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.BranchCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(i => i.AccountNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(i => i.Status)
            .IsRequired()
            .HasConversion<int>();

        // Relationship with CourtCase - CASCADE (primary relationship)
        builder
            .HasOne(i => i.Case)
            .WithMany(c => c.Invoices)
            .HasForeignKey(i => i.CaseId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relationship with User - NO ACTION (to prevent cascade conflicts)
        builder
            .HasOne(i => i.User)
            .WithMany(u => u.Invoices)
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
