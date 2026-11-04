using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.CourtCaseDates.Commands.SetToComplete;

public class SetToCompleteCommandHandler(ICourtCaseDatesService courtCaseDatesService)
    : IRequestHandler<SetToCompleteCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(SetToCompleteCommand request, CancellationToken cancellationToken)
    {
        var result = await courtCaseDatesService.SetToComplete(request.Id, cancellationToken);

        return result;
    }
}
