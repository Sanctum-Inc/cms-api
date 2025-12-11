using Application.Common.Interfaces.Services;
using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.CourtCase.Queries.Get;
public class GetQueryHandler : IRequestHandler<GetQuery, ErrorOr<IEnumerable<CourtCaseResult>>>
{
    private readonly ICourtCaseService _courtCaseService;

    public GetQueryHandler(ICourtCaseService courtCaseService)
    {
        _courtCaseService = courtCaseService;
    }

    public async Task<ErrorOr<IEnumerable<CourtCaseResult>>> Handle(GetQuery request, CancellationToken cancellationToken)
    {
        var result = await _courtCaseService.Get(cancellationToken);

        return result;
    }
}
