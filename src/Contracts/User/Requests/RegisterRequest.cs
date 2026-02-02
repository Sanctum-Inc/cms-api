namespace Contracts.User.Requests;

public record RegisterRequest(
    string Email,
    string Name,
    string Surname,
    string MobileNumber,
    string Password,
    string FirmId);
