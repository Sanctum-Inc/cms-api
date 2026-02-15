using Domain.Lawyers;

namespace Application.Common.Models;

public class LawyerResult
{
    public Guid Id { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string MobileNumber { get; set; }
    public required Speciality Speciality { get; set; }
    public required int TotalCases { get; set; }
}
