namespace Contracts.Lawyer.Responses;

public record LawyerResponse(
    Guid Id,
    string Name,
    string Surname,
    string MobileNumber,
    string Email);
