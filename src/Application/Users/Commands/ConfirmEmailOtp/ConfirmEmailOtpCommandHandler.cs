using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Options;
namespace Application.Users.Commands.ConfirmEmailOtp;

public class ConfirmEmailOtpCommandHandler : IRequestHandler<ConfirmEmailOtpCommand, ErrorOr<string>>
{
    private readonly IUserService _userService;

    public ConfirmEmailOtpCommandHandler(
        IUserService userService)
    {
        _userService = userService;
    }

    public async Task<ErrorOr<string>> Handle(ConfirmEmailOtpCommand request, CancellationToken cancellationToken)
    {
        var result = await _userService.ConfirmEmailOtp(request.Token, request.Email, cancellationToken);

        return result;
    }
}
