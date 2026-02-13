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

    public async Task<Firm?> GetUserFirm(CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_sessionResolver.UserId);
        return await _dbSet
            .Include(f => f.Users)
            .Where(f => f.Users.Any(u => u.Id == userId))
            .OrderByDescending(f => f.Created)
            .FirstOrDefaultAsync(cancellationToken);

    }
}
