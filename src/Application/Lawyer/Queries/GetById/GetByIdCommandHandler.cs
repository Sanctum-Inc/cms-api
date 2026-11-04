using Application.Common.Interfaces.Services;
using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.Lawyer.Queries.GetById;

public class GetByIdCommandHandler(ILawyerService lawyerService)
    : IRequestHandler<GetByIdCommand, ErrorOr<LawyerResult>>
{
    public async Task<ErrorOr<LawyerResult>> Handle(GetByIdCommand request, CancellationToken cancellationToken)
    {
        var result = await lawyerService.GetById(request.Id, cancellationToken);

        return result;
    }
}
