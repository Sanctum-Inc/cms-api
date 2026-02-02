namespace Application.Common.Interfaces.Services;

/// <summary>
///     Provides functionality for generating JSON Web Tokens (JWT) for authenticated users.
/// </summary>
public interface IJwtService
{
    /// <summary>
    ///     Generates a JWT access token containing the specified user information and claims.
    /// </summary>
    /// <param name="role">The role of the user (e.g., Admin, User).</param>
    /// <param name="email">The email address of the user.</param>
    /// <param name="id">The unique identifier of the user.</param>
    /// <param name="name">The first name of the user.</param>
    /// <param name="surname">The last name of the user.</param>
    /// <returns>
    ///     A signed JWT string that can be used for authenticated requests.
    /// </returns>
    string GenerateToken(
        string role,
        string email,
        string id,
        string name,
        string surname,
        string firmId);
}
