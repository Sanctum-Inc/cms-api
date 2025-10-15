using Application.Users.Commands.Login;
using Application.Users.Commands.Register;
using Application.Users.Queries;
using Domain.Users;
using ErrorOr;

namespace Application.Common.Interfaces.Services;
public interface IUserService
{
    Task<ErrorOr<UserResult>> GetUserById(string id, CancellationToken cancellationToken);
    Task<AuthenticationResult?> Login(string username, string password, CancellationToken cancellationToken);
    Task<ErrorOr<bool>> Register(RegisterCommand request, CancellationToken cancellationToken);
}
