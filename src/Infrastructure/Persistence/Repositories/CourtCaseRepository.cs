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

    public Task<CourtCase?> GetByCaseIdAsync(Guid id, Guid userId, CancellationToken cancellationToken)
    {
        return _dbSet
            .FirstOrDefaultAsync(cc => cc.Id == id && cc.UserId == userId, cancellationToken);
    }
}