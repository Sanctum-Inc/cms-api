using System.ComponentModel.DataAnnotations;
using Domain.CourtCases;
using Domain.Documents;
using Domain.InvoiceItems;
using Domain.Lawyers;

namespace Domain.Users;

public class User
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Surname { get; set; } = null!;

    [Required]
    public string MobileNumber { get; set; } = null!;

    [Required]
    public string PasswordHash { get; set; } = null!;

    [Required]
    public string PasswordSalt { get; set; } = null!;

    [Required]
    public DateTime DateCreated { get; set; }


    // Relations
    public List<CourtCase> CourtCases { get; set; } = [];
    public List<Document> Documents { get; set; } = [];
    public List<InvoiceItem> InvoiceItems { get; set; } = [];
    public List<Lawyer> Lawyers { get; set; } = [];
}
