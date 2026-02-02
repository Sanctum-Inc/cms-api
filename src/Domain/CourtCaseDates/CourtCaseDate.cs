using Domain.Common;
using Domain.CourtCases;
using Domain.Lawyers;
using Domain.Users;

namespace Domain.CourtCaseDates;

public class CourtCaseDate : AuditableEntity
{
    public required string Date { get; set; }

    public required string Title { get; set; }
    public required CourtCaseDateTypes Type { get; set; }

    public required Guid CaseId { get; set; }
    public required CourtCase Case { get; set; }

    public required Guid UserId { get; set; }
    public User? User { get; set; }

    // Many-to-Many
    public List<Lawyer> Lawyers { get; set; } = [];
}
