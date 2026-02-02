using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Session;
using Domain.Firms;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class FirmRepository : BaseRepository<Firm>, IFirmRepository
{
    public FirmRepository(IApplicationDBContext context, ISessionResolver sessionResolver) : base(context,
        sessionResolver)
    {
    }

    public async Task<Firm?> GetLatest(CancellationToken cancellationToken)
    {
        return await _dbSet
            .OrderByDescending(f => f.Created)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
