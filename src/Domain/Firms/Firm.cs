// Domain/Firms/Firm.cs
using System.ComponentModel.DataAnnotations;
using Domain.Common;
using Domain.Lawyers;
using Domain.Users;

namespace Domain.Firms;

public class Firm : AuditableEntity
{
    [Required]
    public required string Name { get; set; }

    [Required]
    public required string Address { get; set; }

    [Required]
    public required string Telephone { get; set; }

    [Required]
    public required string Fax { get; set; }

    [Required]
    public required string Mobile { get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    public required DateTime AttorneyAdmissionDate { get; set; }

    [Required]
    public required DateTime AdvocateAdmissionDate { get; set; }

    // Banking Details
    [Required]
    public required string AccountName { get; set; }

    [Required]
    public required string Bank { get; set; }

    [Required]
    public required string BranchCode { get; set; }

    [Required]
    public required string AccountNumber { get; set; }

    // Relations
    public List<User> Users { get; set; } = [];
    public List<Lawyer> Lawyers { get; set; } = [];
}
