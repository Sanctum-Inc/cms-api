using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Session;
using Domain.CourtCases;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CourtCaseRepository : BaseRepository<CourtCase>, ICourtCaseRepository
{
    public CourtCaseRepository(IApplicationDBContext context, ISessionResolver sessionResolver) : base(context, sessionResolver) { }

    public async Task<IEnumerable<CourtCase>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _context
            .Set<CourtCase>()
            .Where(cc => cc.UserId == userId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<CourtCase?> GetByIdAndUserIdAsync(Guid caseId, Guid userId, CancellationToken cancellationToken)
    {
        return await _context
            .Set<CourtCase>()
            .Where(cc => cc.Id == caseId && cc.UserId == userId)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }
}