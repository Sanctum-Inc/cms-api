using Application.Common.Interfaces.Services;
using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.CourtCase.Queries.Get;
public class GetCommandHandler : IRequestHandler<GetCommand, ErrorOr<IEnumerable<CourtCaseResult>>>
{
    private readonly ICourtCaseService _courtCaseService;

    public GetCommandHandler(ICourtCaseService courtCaseService)
    {
        _courtCaseService = courtCaseService;
    }

    public async Task<ErrorOr<IEnumerable<CourtCaseResult>>> Handle(GetCommand request, CancellationToken cancellationToken)
    {
        var result = await _courtCaseService.Get(cancellationToken);

        return result;
    }
}
