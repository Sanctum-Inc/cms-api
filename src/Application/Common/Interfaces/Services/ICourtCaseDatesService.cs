using Application.Common.Models;
using ErrorOr;

namespace Application.Common.Interfaces.Services;

public interface ICourtCaseDatesService : IBaseService<CourtCaseDateResult>
{
    Task<ErrorOr<CourtCaseDateResult>?> GetCourtCaseDateInformation(CancellationToken cancellationToken);
    Task<ErrorOr<bool>> SetToComplete(Guid id, CancellationToken cancellationToken);
    Task<ErrorOr<bool>> SetToCancelled(Guid id, CancellationToken cancellationToken);
}
