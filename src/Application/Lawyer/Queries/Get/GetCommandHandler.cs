using Application.Common.Interfaces.Services;
using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.Lawyer.Queries.Get;

public class GetCommandHandler : IRequestHandler<GetCommand, ErrorOr<IEnumerable<LawyerResult>>>
{
    private readonly ILawyerService _lawyerService;

    public GetCommandHandler(ILawyerService lawyerService)
    {
        _lawyerService = lawyerService;
    }

    public async Task<ErrorOr<IEnumerable<LawyerResult>>> Handle(GetCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _lawyerService.Get(cancellationToken);

        return result;
    }
}
