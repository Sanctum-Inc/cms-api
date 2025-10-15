using System.ComponentModel.DataAnnotations;
using Domain.Common;
using Domain.CourtCaseDates;
using Domain.CourtCases;
using Domain.Users;

namespace Domain.Lawyers;

public class Lawyer : AuditableEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public required string Surname { get; set; }

    [Required]
    public required string MobileNumber { get; set; }

    [Required]
    public Guid UserId { get; set; }
    public required User User { get; set; }

    // Relations
    public List<CourtCase> CourtCases { get; set; } = [];
    public List<CourtCaseDate> CourtCaseDates { get; set; } = [];
}
