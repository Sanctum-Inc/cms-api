using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Users.Commands.Register;
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<bool>>
{
    private readonly IUserService _userService;

    public RegisterCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<ErrorOr<bool>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var result = await _userService.Register(request, cancellationToken);

        return result;
    }
}
