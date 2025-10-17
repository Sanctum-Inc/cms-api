namespace Contracts.Lawyer.Responses;
public record GetLawyerResponse(
    Guid Id,
    string Name,
    string Surname,
    string MobileNumber,
    string Email);