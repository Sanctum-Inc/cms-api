using System.ComponentModel.DataAnnotations;
using Domain.Common;
using Domain.CourtCaseDates;
using Domain.CourtCases;
using Domain.Users;

namespace Domain.Lawyers;

public class Lawyer : AuditableEntity
{
    [Required] [EmailAddress] public required string Email { get; set; }

    [Required] public required string Name { get; set; }

    [Required] public required string Surname { get; set; }

    [Required] public required string MobileNumber { get; set; }

    public string? FirmName { get; set; } // Just for reference, not a FK

    public required Speciality Specialty { get; set; }

    // Created by user (for tracking purposes)
    [Required] public required Guid CreatedByUserId { get; set; }

    public User? CreatedByUser { get; set; }

    // Relations - Many-to-Many with CourtCases and CourtCaseDates
    public List<CourtCase> CourtCases { get; set; } = [];
    public List<CourtCaseDate> CourtCaseDates { get; set; } = [];
}
