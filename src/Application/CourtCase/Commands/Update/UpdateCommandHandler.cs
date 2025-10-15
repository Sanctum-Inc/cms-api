using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.CourtCase.Commands.Update;
public class UpdateCommandHandler : IRequestHandler<UpdateCommand, ErrorOr<bool>>
{
    public ICourtCaseService _courtCaseService;

    public UpdateCommandHandler(ICourtCaseService courtCaseService)
    {
        _courtCaseService = courtCaseService;
    }

    public Task<ErrorOr<bool>> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        var result = _courtCaseService.Update(request, cancellationToken);

        return result;
    }
}
