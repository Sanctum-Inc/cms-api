namespace Contracts.User.Responses;

public record LoginResponse(
    string Token,
    DateTime Expiration,
    string RefreshToken);
