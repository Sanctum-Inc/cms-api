using System.ComponentModel.DataAnnotations;
using Domain.Common;
using Domain.CourtCases;
using Domain.Lawyers;

namespace Domain.CourtCaseDates;

public class CourtCaseDate : AuditableEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public required string Date { get; set; }

    [Required]
    public required string Title { get; set; }

    [Required]
    public Guid CaseId { get; set; }
    public required CourtCase Case { get; set; }

    // Many-to-Many
    public List<Lawyer> Lawyers { get; set; } = [];
}
