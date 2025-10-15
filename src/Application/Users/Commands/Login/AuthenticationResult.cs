namespace Application.Users.Commands.Login;
public class AuthenticationResult
{
    public string Token { get; }
    public string RefreshToken { get; }
    public DateTime Expiration { get; }

    public AuthenticationResult(DateTime expiration, string token, string refreshToken)
    {
        Expiration = expiration;
        Token = token;
        RefreshToken = refreshToken;
    }
}