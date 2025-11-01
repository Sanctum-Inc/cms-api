using Application.Common.Interfaces.Services;
using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.CourtCaseDates.Queries.Get;
public class GetCommandHandler : IRequestHandler<GetCommand, ErrorOr<IEnumerable<CourtCaseDateResult>>>
{
    private readonly ICourtCaseDatesService _courtCaseDatesService;

    public GetCommandHandler(ICourtCaseDatesService courtCaseDatesService)
    {
        _courtCaseDatesService = courtCaseDatesService;
    }

    public async Task<ErrorOr<IEnumerable<CourtCaseDateResult>>> Handle(GetCommand request, CancellationToken cancellationToken)
    {
        var result = await _courtCaseDatesService.Get(cancellationToken);

        return result;
    }
}
