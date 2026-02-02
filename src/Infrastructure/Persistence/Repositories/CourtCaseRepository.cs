using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Session;
using Domain.CourtCases;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CourtCaseRepository : BaseRepository<CourtCase>, ICourtCaseRepository
{
    public CourtCaseRepository(IApplicationDBContext context, ISessionResolver sessionResolver) : base(context,
        sessionResolver)
    {
    }

    public Task<CourtCase?> GetByCaseIdAsync(Guid id, Guid userId, CancellationToken cancellationToken)
    {
        return _dbSet
            .FirstOrDefaultAsync(cc => cc.Id == id && cc.UserId == userId, cancellationToken);
    }

    public override async Task<IEnumerable<CourtCase>> GetAll(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.User)
            .Include(c => c.CourtCaseDates)
            .ThenInclude(d => d.Lawyers)
            .Include(c => c.Documents)
            .Include(c => c.Invoices)
            .ThenInclude(i => i.Items)
            .Include(c => c.Lawyers)
            .Where(x => x.UserId.ToString() == _sessionResolver.UserId)
            .ToListAsync(cancellationToken);
    }

    public override async Task<CourtCase?> GetByIdAndUserIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.User)
            .Include(c => c.CourtCaseDates)
            .ThenInclude(d => d.Lawyers)
            .Include(c => c.Documents)
            .Include(c => c.Invoices)
            .ThenInclude(i => i.Items)
            .Include(c => c.Lawyers)
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId.ToString() == _sessionResolver.UserId, cancellationToken);
    }
}
