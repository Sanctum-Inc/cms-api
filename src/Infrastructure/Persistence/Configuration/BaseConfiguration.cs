using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class BaseConfiguration<T> : IEntityTypeConfiguration<T>
    where T : AuditableEntity
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        // 👇 Primary key (assuming all entities have an Id)
        builder.HasKey("Id");

        // 👇 Created timestamp
        builder.Property(e => e.Created)
            .IsRequired()
            .ValueGeneratedOnAdd();

        // 👇 CreatedBy user
        builder.Property(e => e.CreatedBy)
            .HasMaxLength(256)
            .IsRequired(false);

        // 👇 LastModified timestamp
        builder.Property(e => e.LastModified)
            .IsRequired(false)
            .ValueGeneratedOnAddOrUpdate();

        // 👇 LastModifiedBy user
        builder.Property(e => e.LastModifiedBy)
            .HasMaxLength(256)
            .IsRequired(false);

        // 👇 Soft delete flag
        builder.Property(e => e.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();
    }
}
