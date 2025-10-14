using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.CourtCase.Queries.Get;
public class GetCommandHandler : IRequestHandler<GetCommand, ErrorOr<GetCourtCaseResult>>
{
    private readonly ICourtCaseService _courtCaseService;

    public GetCommandHandler(ICourtCaseService courtCaseService)
    {
        _courtCaseService = courtCaseService;
    }

    public async Task<ErrorOr<GetCourtCaseResult>> Handle(GetCommand request, CancellationToken cancellationToken)
    {
        var result = await _courtCaseService.Get(cancellationToken);

        return result;
    }
}
