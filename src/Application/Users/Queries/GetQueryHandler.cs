using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Users.Queries;
public class GetQueryHandler : IRequestHandler<GetQuery, ErrorOr<UserResult>>
{
    private readonly IUserService _userService;

    public GetQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<ErrorOr<UserResult>> Handle(GetQuery request, CancellationToken cancellationToken)
    {
        var result = await _userService.GetUserById(request.Id, cancellationToken);

        return result;
    }
}
