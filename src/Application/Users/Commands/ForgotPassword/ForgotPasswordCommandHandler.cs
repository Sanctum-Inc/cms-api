using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Users.Commands.ForgotPassword;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, ErrorOr<bool>>
{
    private readonly IUserService _userService;

    public ForgotPasswordCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<ErrorOr<bool>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var result = await _userService.InitiateForgotPassword(request.Email, cancellationToken);

        return result;
    }
}
