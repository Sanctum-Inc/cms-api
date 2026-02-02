using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.CourtCaseDates.Commands.Update;

public class UpdateCommandHandler : IRequestHandler<UpdateCommand, ErrorOr<bool>>
{
    private readonly ICourtCaseDatesService _courtCaseDatesService;

    public UpdateCommandHandler(ICourtCaseDatesService courtCaseDatesService)
    {
        _courtCaseDatesService = courtCaseDatesService;
    }

    public async Task<ErrorOr<bool>> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        var result = await _courtCaseDatesService.Update(request, cancellationToken);

        return result;
    }
}
