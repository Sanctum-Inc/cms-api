using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.CourtCase.Commands.Delete;

public class DeleteCommandHandler : IRequestHandler<DeleteCommand, ErrorOr<bool>>
{
    private readonly ICourtCaseService _courtCaseService;

    public DeleteCommandHandler(ICourtCaseService courtCaseService)
    {
        _courtCaseService = courtCaseService;
    }

    public Task<ErrorOr<bool>> Handle(DeleteCommand request, CancellationToken cancellationToken)
    {
        var result = _courtCaseService.Delete(request.Id, cancellationToken);

        return result;
    }
}
