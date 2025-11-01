using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.CourtCaseDates.Commands.Delete;
public class DeleteCommandHandler : IRequestHandler<DeleteCommand, ErrorOr<bool>>
{
    private readonly ICourtCaseDatesService _courtCaseDatesService;

    public DeleteCommandHandler(ICourtCaseDatesService courtCaseDatesService)
    {
        _courtCaseDatesService = courtCaseDatesService;
    }

    public async Task<ErrorOr<bool>> Handle(DeleteCommand request, CancellationToken cancellationToken)
    {
        var result = await _courtCaseDatesService.Delete(request.Id, cancellationToken);

        return result;
    }
}
