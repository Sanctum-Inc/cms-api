using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Lawyer.Commands.Delete;

public class DeleteCommandHandler : IRequestHandler<DeleteCommand, ErrorOr<bool>>
{
    private readonly ILawyerService _lawyerService;

    public DeleteCommandHandler(ILawyerService lawyerService)
    {
        _lawyerService = lawyerService;
    }

    public async Task<ErrorOr<bool>> Handle(DeleteCommand request, CancellationToken cancellationToken)
    {
        var result = await _lawyerService.Delete(request.Id, cancellationToken);

        return result;
    }
}
