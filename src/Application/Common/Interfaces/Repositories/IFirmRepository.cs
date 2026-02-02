namespace Application.Common.Interfaces.Repositories;

public interface IFirmRepository : IBaseRepository<Domain.Firms.Firm>
{
    Task<Domain.Firms.Firm?> GetLatest(CancellationToken cancellationToken);
}
