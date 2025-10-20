
using Domain.Firms;

namespace Application.Common.Interfaces.Repositories;
public interface IFirmRepository : IBaseRepository<Domain.Firms.Firm>
{
    Task<Firm?> GetLatest(CancellationToken cancellationToken);
}
