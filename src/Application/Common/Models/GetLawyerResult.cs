using Application.Lawyer.Queries.GetById;

namespace Application.Common.Models;
public class GetLawyerResult
{
    public Guid Id { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string MobileNumber { get; set; }
}
