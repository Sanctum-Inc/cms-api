namespace Contracts.Lawyer.Requests;

public record AddLawyerRequest(
    string Email,
    string Name,
    string Surname,
    string MobileNumber,
    int Specialty
);
