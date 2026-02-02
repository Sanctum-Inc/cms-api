using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Lawyer.Commands.Update;

public class UpdateCommandHandler : IRequestHandler<UpdateCommand, ErrorOr<bool>>
{
    private readonly ILawyerService _lawyerService;

    public UpdateCommandHandler(ILawyerService lawyerService)
    {
        _lawyerService = lawyerService;
    }

    public async Task<ErrorOr<bool>> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        var result = await _lawyerService.Update(
            request,
            cancellationToken);

        return result;
    }
}
