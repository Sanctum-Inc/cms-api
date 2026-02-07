using Application.Common.Models;
using ErrorOr;

namespace Application.Common.Interfaces.Services;

public interface IDashboardService
{
    public Task<ErrorOr<DashBoardResult>> GetDashboardInformation(CancellationToken cancellationToken);
}
