namespace Contracts.Lawyer.Requests;
public record UpdateLawyerRequest(
    string Email,
    string Name,
    string Surname,
    string MobileNumber,
    string Specialty
);
