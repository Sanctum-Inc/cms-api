using Application.Common.Interfaces.Services;
using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.CourtCaseDates.Queries.GetById;
public class GetByIdCommandHandler : IRequestHandler<GetByIdCommand, ErrorOr<CourtCaseDateResult>>
{
    private readonly ICourtCaseDatesService _courtCaseDatesService;

    public GetByIdCommandHandler(ICourtCaseDatesService courtCaseDatesService)
    {
        _courtCaseDatesService = courtCaseDatesService;
    }

    public async Task<ErrorOr<CourtCaseDateResult>> Handle(GetByIdCommand request, CancellationToken cancellationToken)
    {
        var result = await _courtCaseDatesService.GetById(request.Id, cancellationToken);

        return result;
    }
}
