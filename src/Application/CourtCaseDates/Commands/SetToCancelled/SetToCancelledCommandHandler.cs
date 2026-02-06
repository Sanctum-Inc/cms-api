using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.CourtCaseDates.Commands.SetToCancelled;

public class SetToCancelledCommandHandler : IRequestHandler<SetToCancelledCommand, ErrorOr<bool>>
{
    private readonly ICourtCaseDatesService _courtCaseDatesService;

    public SetToCancelledCommandHandler(ICourtCaseDatesService courtCaseDatesService)
    {
        _courtCaseDatesService = courtCaseDatesService;
    }

    public async Task<ErrorOr<bool>> Handle(SetToCancelledCommand request, CancellationToken cancellationToken)
    {
        var result = await _courtCaseDatesService.SetToCancelled(request.Id, cancellationToken);

        return result;
    }
}
