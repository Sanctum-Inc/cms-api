using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Session;
using Domain.Lawyers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class LawyerRepository : BaseRepository<Lawyer>, ILawyerRepository
{
    public LawyerRepository(IApplicationDBContext context, ISessionResolver sessionResolver) : base(context,
        sessionResolver)
    {
    }

    public override async Task<IEnumerable<Lawyer>> GetAll(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.CourtCases)
            .Where(x => x.UserId.ToString() == _sessionResolver.UserId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Lawyer?> GetLawyerReportInformation(Guid id, CancellationToken cancellationToken)
    {
        return await _dbSet
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.CourtCases)
            .ThenInclude(x => x.Invoices)
            .ThenInclude(x => x.Items)
            .Include(x => x.CourtCaseDates)
            .FirstOrDefaultAsync(x =>
                    x.UserId.ToString() == _sessionResolver.UserId &&
                    x.Id == id,
                cancellationToken);
    }
}
