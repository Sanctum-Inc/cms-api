using Domain.Documents;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Configuration;
public class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Document> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Path)
            .IsRequired();

        builder.Property(d => d.FileName)
            .IsRequired();

        builder.Property(d => d.DateCreated);

        builder.HasOne(d => d.User)
            .WithMany(u => u.Documents)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
