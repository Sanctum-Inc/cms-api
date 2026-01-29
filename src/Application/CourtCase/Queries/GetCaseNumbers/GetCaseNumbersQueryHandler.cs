using Application.Common.Interfaces.Services;
using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.CourtCase.Queries.GetCaseNumbers;
public class GetCaseNumbersQueryHandler : IRequestHandler<GetCaseNumbersQuery, ErrorOr<IEnumerable<string>?>>
{
    private readonly ICourtCaseService _courtCaseService;

    public GetCaseNumbersQueryHandler(ICourtCaseService courtCaseService)
    {
        _courtCaseService = courtCaseService;
    }

    public Task<ErrorOr<IEnumerable<string>?>> Handle(GetCaseNumbersQuery request, CancellationToken cancellationToken)
    {
        var result = _courtCaseService.GetCaseNumbers(cancellationToken);

        return result;
    }
}
