using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;
internal class UserConfiguration : IEntityTypeConfiguration<Domain.Users.User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Name)
            .IsRequired();

        builder.Property(u => u.Surname)
            .IsRequired();

        builder.Property(u => u.Email)
            .IsRequired();

        builder.HasMany(u => u.CourtCases)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId);

        builder.HasMany(u => u.Documents)
            .WithOne(d => d.User)
            .HasForeignKey(d => d.UserId);

        builder.HasMany(u => u.Invoices)
            .WithOne(i => i.User)
            .HasForeignKey(i => i.UserId);

        builder.HasMany(u => u.Lawyers)
            .WithOne(l => l.User)
            .HasForeignKey(l => l.UserId);

        builder.HasOne(x => x.Firm)
            .WithMany(p => p.Users)
            .HasForeignKey(x => x.FirmId);
    }
}
