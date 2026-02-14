using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Users.Commands.ResendConfirmEmailOtp;

public class ResendConfirmEmailOtpCommandHandler : IRequestHandler<ResendConfirmEmailOtpCommand, ErrorOr<bool>>
{
    private readonly IUserService _userService;

    public ResendConfirmEmailOtpCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<ErrorOr<bool>> Handle(ResendConfirmEmailOtpCommand request, CancellationToken cancellationToken)
    {
        var result = await _userService.ResendConfirmEmailOtp(request.Email, cancellationToken);

        return result;
    }
}
