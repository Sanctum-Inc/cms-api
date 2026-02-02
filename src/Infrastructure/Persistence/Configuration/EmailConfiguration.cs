using Domain.Emails;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class EmailConfiguration : BaseConfiguration<Email>
{
    public override void Configure(EntityTypeBuilder<Email> builder)
    {
        // IMPORTANT: Call base configuration FIRST
        base.Configure(builder);

        // Recipients
        builder.Property(e => e.ToAddresses)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(e => e.CcAddresses)
            .HasMaxLength(2000);

        builder.Property(e => e.BccAddresses)
            .HasMaxLength(2000);

        // Content
        builder.Property(e => e.Subject)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.Body)
            .IsRequired();

        builder.Property(e => e.IsHtml)
            .IsRequired();

        // Attachments
        builder.Property(e => e.AttachmentIds)
            .HasMaxLength(4000);

        // Status
        builder.Property(e => e.Status)
            .HasDefaultValue(EmailStatus.Pending)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(e => e.RetryCount)
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(e => e.ErrorMessage)
            .HasMaxLength(4000);

        builder.Property(e => e.SentAt);

        // 🔗 CRITICAL: Relationship with User (NO cascade delete)
        builder
            .HasOne(e => e.User)
            .WithMany(u => u.Emails)
            .HasForeignKey(e => e.CreatedBy)
            .OnDelete(DeleteBehavior.NoAction);

        // Indexes
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.CreatedBy);
        builder.HasIndex(e => new { e.Status, e.CreatedBy });
    }
}
