using Application.Common.Interfaces.Services;
using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.Dashboard.Queries.GetDashboardInformation;

public class GetDashboardInformationQueryHandler : IRequestHandler<GetDashboardInformationQuery, ErrorOr<DashBoardResult>>
{
    private readonly IDashboardService _dashboardService;

    public GetDashboardInformationQueryHandler(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public async Task<ErrorOr<DashBoardResult>> Handle(GetDashboardInformationQuery request, CancellationToken cancellationToken)
    {
        var result = await _dashboardService.GetDashboardInformation(cancellationToken);

        return result;
    }
}
