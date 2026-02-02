using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.CourtCase.Commands.Add;

public class AddCommandHandler : IRequestHandler<AddCommand, ErrorOr<Guid>>
{
    private readonly ICourtCaseService _courtCaseService;

    public AddCommandHandler(ICourtCaseService courtCaseService)
    {
        _courtCaseService = courtCaseService;
    }

    public async Task<ErrorOr<Guid>> Handle(AddCommand request, CancellationToken cancellationToken)
    {
        var result = await _courtCaseService.Add(request, cancellationToken);

        return result;
    }
}
