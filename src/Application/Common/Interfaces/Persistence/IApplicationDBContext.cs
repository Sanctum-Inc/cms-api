using Domain.CourtCaseDates;
using Domain.Documents;
using Domain.InvoiceItems;
using Domain.Lawyers;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces.Persistence;

public interface IApplicationDBContext
{
    // Expose DbSets for specific entities
    DbSet<User> Users { get; }
    DbSet<Domain.CourtCases.CourtCase> CourtCases { get; }
    DbSet<Domain.Documents.Document> Documents { get; }
    DbSet<InvoiceItem> InvoiceItems { get; }
    DbSet<Lawyer> Lawyers { get; }
    DbSet<CourtCaseDate> CourtCaseDates { get; }

    // ✅ This allows generic access for repositories
    DbSet<TEntity> Set<TEntity>() where TEntity : class;

    // ✅ Save changes to the database
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
