using Application.Common.Interfaces.Services;
using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.Lawyer.Queries.GetById;

public class GetByIdCommandHandler : IRequestHandler<GetByIdCommand, ErrorOr<LawyerResult?>>
{
    private readonly ILawyerService _lawyerService;

    public GetByIdCommandHandler(ILawyerService lawyerService)
    {
        _lawyerService = lawyerService;
    }

    public async Task<ErrorOr<LawyerResult?>> Handle(GetByIdCommand request, CancellationToken cancellationToken)
    {
        var result = await _lawyerService.GetById(request.Id, cancellationToken);

        return result;
    }
}
