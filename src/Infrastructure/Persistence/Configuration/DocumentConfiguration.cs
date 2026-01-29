using Domain.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        // Primary key
        builder.HasKey(d => d.Id);

        // Properties
        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(d => d.FileName)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(d => d.ContentType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.Size)
            .IsRequired();

        // Relationships

        // Document -> User (many-to-one)
        builder.HasOne(d => d.User)
            .WithMany(u => u.Documents)
            .HasForeignKey(d => d.UserId);

        // Document -> CourtCase (many-to-one)
        builder.HasOne(d => d.Case)
            .WithMany(c => c.Documents)
            .HasForeignKey(d => d.CaseId);
    }
}
