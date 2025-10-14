using Application.Common.Interfaces.Services;
using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.CourtCase.Queries.GetById;
public class GetByIdCommandHandler : IRequestHandler<GetByIdCommand, ErrorOr<CourtCaseResult>>
{
    private readonly ICourtCaseService _courtCaseService;
    public GetByIdCommandHandler(ICourtCaseService courtCaseService)
    {
        _courtCaseService = courtCaseService;
    }

    public async Task<ErrorOr<CourtCaseResult>> Handle(GetByIdCommand request, CancellationToken cancellationToken)
    {
        var result = await _courtCaseService.GetById(request.Id, cancellationToken);

        return result;
    }
}
