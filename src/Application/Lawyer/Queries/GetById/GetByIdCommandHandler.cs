using Application.Common.Interfaces.Services;
using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.Lawyer.Queries.GetById;
public class GetByIdCommandHandler : IRequestHandler<GetByIdCommand, ErrorOr<GetLawyerResult>>
{
    private readonly ILawyerService _lawyerService;

    public GetByIdCommandHandler(ILawyerService lawyerService)
    {
        _lawyerService = lawyerService;
    }
    public async Task<ErrorOr<GetLawyerResult>> Handle(GetByIdCommand request, CancellationToken cancellationToken)
    {
        var result = await _lawyerService.GetById(request.Id, cancellationToken);

        return result;
    }
}
