namespace Contracts.User.Responses;

public record UserResponse(
    Guid Id,
    string Email,
    string Name,
    string Surname,
    string MobileNumber,
    DateTime DateCreated);
