using System.ComponentModel.DataAnnotations;
using Domain.CourtCaseDates;
using Domain.Documents;
using Domain.InvoiceItems;
using Domain.Lawyers;
using Domain.Users;

public class CourtCase
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public required string CaseNumber { get; set; }
    [Required]
    public required string Location { get; set; }
    [Required]
    public required string Plaintiff { get; set; }
    [Required]
    public required string Defendant { get; set; }
    [Required]
    public required string Status { get; set; }
    public string? Type { get; set; }
    public string? Outcome { get; set; }
    public DateTime DateCreated { get; set; }
    [Required]
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public List<CourtCaseDate> CourtCaseDates { get; set; } = new();
    public List<Document> Documents { get; set; } = new();
    public List<InvoiceItem> InvoiceItems { get; set; } = new();
    public List<Lawyer> Lawyers { get; set; } = new();
}
