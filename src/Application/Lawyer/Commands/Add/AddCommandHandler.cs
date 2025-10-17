using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Lawyer.Commands.Add;
public class AddCommandHandler : IRequestHandler<AddCommand, ErrorOr<bool>>
{
    private readonly ILawyerService _lawyerService;

    public AddCommandHandler(ILawyerService lawyerService)
    {
        _lawyerService = lawyerService;
    }

    public async Task<ErrorOr<bool>> Handle(AddCommand request, CancellationToken cancellationToken)
    {
        var result = await _lawyerService.Add(request, cancellationToken);

        return result;
    }
}
