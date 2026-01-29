using Domain.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class DocumentConfiguration : BaseConfiguration<Document>
{
    public override void Configure(EntityTypeBuilder<Document> builder)
    {
        // IMPORTANT: Call base configuration FIRST
        base.Configure(builder);

        // Then add entity-specific configuration
        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(d => d.FileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(d => d.ContentType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.Size)
            .IsRequired();

        // Relationship with CourtCase - CASCADE (primary relationship)
        builder
            .HasOne(d => d.Case)
            .WithMany(c => c.Documents)
            .HasForeignKey(d => d.CaseId)
            .OnDelete(DeleteBehavior.Cascade);

        // CRITICAL: Relationship with User - NO ACTION (to prevent cascade conflicts)
        builder
            .HasOne(d => d.User)
            .WithMany(u => u.Documents)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
