using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Session;
using Domain.CourtCaseDates;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CourtCaseDateRepository : BaseRepository<CourtCaseDate>, ICourtCaseDateRepository
{
    public CourtCaseDateRepository(IApplicationDBContext context, ISessionResolver sessionResolver) : base(context,
        sessionResolver)
    {
    }

    public override async Task<IEnumerable<CourtCaseDate>> GetAll(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.Case)
            .Where(x => EF.Property<Guid>(x, "UserId") == new Guid(_sessionResolver.UserId!))
            .ToListAsync(cancellationToken);
    }
}
