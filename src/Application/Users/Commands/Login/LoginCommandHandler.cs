using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Users.Commands.Login;
public class LoginCommandHandler : IRequestHandler<LoginCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IUserService _userService;

    public LoginCommandHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await _userService.Login(request.Email, request.Password, cancellationToken);

        return result;
    }
}
