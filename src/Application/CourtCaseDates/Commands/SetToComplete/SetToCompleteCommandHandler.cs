using Application.Common.Interfaces.Services;
using Application.CourtCaseDates.Commands.SetToCancelled;
using ErrorOr;
using MediatR;

namespace Application.CourtCaseDates.Commands.SetToComplete;

public class SetToCompleteCommandHandler : IRequestHandler<SetToCompleteCommand, ErrorOr<bool>>
{
    private readonly ICourtCaseDatesService _courtCaseDatesService;

    public SetToCompleteCommandHandler(ICourtCaseDatesService courtCaseDatesService)
    {
        _courtCaseDatesService = courtCaseDatesService;
    }

    public async Task<ErrorOr<bool>> Handle(SetToCompleteCommand request, CancellationToken cancellationToken)
    {
        var result = await _courtCaseDatesService.SetToComplete(request.Id, cancellationToken);

        return result;
    }
}
