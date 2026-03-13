namespace Contracts.User.Requests;

public record ChangePasswordRequest(
    string Token,
    string Password);
