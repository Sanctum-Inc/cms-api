using Application.Common.Interfaces.Services;
using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.Users.Queries.GetQuery;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, ErrorOr<UserResult>>
{
    private readonly IUserService _userService;

    public GetUsersQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<ErrorOr<UserResult>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var result = await _userService.GetUserById(request.Id, cancellationToken);

        return result;
    }
}
