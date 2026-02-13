using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.CourtCase.Queries.GetCourseCaseInformation;

public class GetCourtCaseInformationQueryHandler : IRequestHandler<GetCourtCaseInformationQuery, ErrorOr<CourtCaseInformationResult>>
{
    private readonly ICourtCaseService _courtCaseService;

    public GetCourtCaseInformationQueryHandler(
        ICourtCaseService courtCaseService)
    {
        _courtCaseService = courtCaseService;
    }

    public async Task<ErrorOr<CourtCaseInformationResult>> Handle(GetCourtCaseInformationQuery request, CancellationToken cancellationToken)
    {
        var result = await _courtCaseService.GetCourtCaseInformationById(request.Id, cancellationToken);

        return result;
    }
}
