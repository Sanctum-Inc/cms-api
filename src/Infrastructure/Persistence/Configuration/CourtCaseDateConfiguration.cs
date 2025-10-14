using Domain.CourtCaseDates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;
public class CourtCaseDateConfiguration : IEntityTypeConfiguration<CourtCaseDate>
{
    public void Configure(EntityTypeBuilder<CourtCaseDate> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Date)
            .IsRequired();

        builder.Property(d => d.Title)
            .IsRequired();

        builder.Property(d => d.CaseId)
            .IsRequired();

        builder.HasOne(d => d.Case)
            .WithMany(c => c.CourtCaseDates)
            .HasForeignKey(d => d.CaseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
