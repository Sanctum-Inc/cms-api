// Domain/Users/User.cs
using System.ComponentModel.DataAnnotations;
using Domain.Common;
using Domain.CourtCaseDates;
using Domain.CourtCases;
using Domain.Documents;
using Domain.Firms;
using Domain.Invoices;
using Domain.Lawyers;

namespace Domain.Users;

public class User : AuditableEntity
{
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
    public required string PasswordHash { get; set; }

    [Required]
    public required string PasswordSalt { get; set; }

    [Required]
    public required UserRole Role { get; set; } = UserRole.FirmUser;

    // Relations
    [Required]
    public required Guid FirmId { get; set; }
    public Firm? Firm { get; set; }

    public List<CourtCase> CourtCases { get; set; } = [];
    public List<CourtCaseDate> CourtCasesDates { get; set; } = [];
    public List<Document> Documents { get; set; } = [];
    public List<Invoice> Invoices { get; set; } = [];
    public List<Lawyer> Lawyers { get; set; } = []; // Lawyers created by this user
}
