using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Users.Commands.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ErrorOr<bool>>
{
    private readonly IUserService _userService;

    public ChangePasswordCommandHandler(
        IUserService userService)
    {
        _userService = userService;
    }


    public async Task<ErrorOr<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var result = await _userService.ChangePassword(request.Token, request.Password, cancellationToken);

        return result;
    }
}
