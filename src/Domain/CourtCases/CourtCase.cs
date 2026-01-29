using System.ComponentModel.DataAnnotations;
using Domain.Common;
using Domain.CourtCaseDates;
using Domain.Documents;
using Domain.Invoices;
using Domain.Lawyers;
using Domain.Users;

namespace Domain.CourtCases;
public class CourtCase : AuditableEntity
{
    [Required]
    public required string CaseNumber { get; set; }

    [Required]
    public required string Location { get; set; }

    [Required]
    public required string Plaintiff { get; set; }

    [Required]
    public required string Defendant { get; set; }

    [Required]
    public required CourtCaseStatus Status { get; set; }

    public required string Type { get; set; }

    public string? Outcome { get; set; }

    public required bool IsPaid { get; set; }

    // Foreign Keys
    [Required]
    public required Guid UserId { get; set; }
    public User? User { get; set; }

    // Relations
    public List<CourtCaseDate> CourtCaseDates { get; set; } = [];
    public List<Document> Documents { get; set; } = [];
    public List<Invoice> Invoices { get; set; } = [];
    public List<Lawyer> Lawyers { get; set; } = []; // Many-to-Many with external lawyers
}
