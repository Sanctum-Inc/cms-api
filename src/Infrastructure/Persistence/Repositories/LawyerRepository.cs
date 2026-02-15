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
}
