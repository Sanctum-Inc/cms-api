using Application.Common.Interfaces.Services;
using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.Lawyer.Queries.GetReportInformation;

public class GetReportInformationQueryHandler : IRequestHandler<GetReportInformationQuery, ErrorOr<LawyerReportResult>>
{
    private readonly ILawyerService _lawyerService;

    public GetReportInformationQueryHandler(ILawyerService lawyerService)
    {
        _lawyerService = lawyerService;
    }

    public Task<ErrorOr<LawyerReportResult>> Handle(GetReportInformationQuery request, CancellationToken cancellationToken)
    {
        var result = _lawyerService.GetLawyerReport(request.LawyerId, cancellationToken);

        return result;
    }
}
