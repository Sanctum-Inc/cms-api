using Application.Users.Commands.Login;
using Application.Users.Commands.Register;
using Application.Users.Queries;
using ErrorOr;

namespace Application.Common.Interfaces.Services;

/// <summary>
///     Provides a set of operations for managing user authentication and retrieval.
/// </summary>
public interface IUserService
{
    /// <summary>
    ///     Retrieves a user's details by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user to retrieve.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>
    ///     An <see cref="ErrorOr{T}" /> result containing the <see cref="UserResult" /> if the user exists,
    ///     or an error if the user could not be found or the operation failed.
    /// </returns>
    Task<ErrorOr<UserResult>> GetUserById(string id, CancellationToken cancellationToken);

    /// <summary>
    ///     Authenticates a user using their credentials.
    /// </summary>
    /// <param name="username">The username of the user attempting to log in.</param>
    /// <param name="password">The password of the user attempting to log in.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>
    ///     An <see cref="ErrorOr{T}" /> result containing an <see cref="AuthenticationResult" /> if authentication succeeds,
    ///     or an error if the credentials are invalid or the operation failed.
    /// </returns>
    Task<ErrorOr<AuthenticationResult>> Login(string username, string password, CancellationToken cancellationToken);

    /// <summary>
    ///     Registers a new user in the system.
    /// </summary>
    /// <param name="request">The registration command containing the new user's information.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>
    ///     An <see cref="ErrorOr{T}" /> result containing <c>true</c> if the registration was successful,
    ///     or an error if the operation failed (e.g., duplicate username or invalid data).
    /// </returns>
    Task<ErrorOr<bool>> Register(RegisterCommand request, CancellationToken cancellationToken);
}
