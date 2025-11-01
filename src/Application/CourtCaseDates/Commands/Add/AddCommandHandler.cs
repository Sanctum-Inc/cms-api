using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.CourtCaseDates.Commands.Add;
public class AddCommandHandler : IRequestHandler<AddCommand, ErrorOr<bool>>
{
    private readonly ICourtCaseDatesService _courtCaseDatesService;

    public AddCommandHandler(ICourtCaseDatesService courtCaseDatesService)
    {
        _courtCaseDatesService = courtCaseDatesService;
    }

    public async Task<ErrorOr<bool>> Handle(AddCommand request, CancellationToken cancellationToken)
    {
        var result = await _courtCaseDatesService.Add(request, cancellationToken);
        return result;
    }
}
