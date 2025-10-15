using System.ComponentModel.DataAnnotations;
using Domain.Common;
using Domain.CourtCases;
using Domain.Users;

namespace Domain.InvoiceItems;

public class InvoiceItem : AuditableEntity
{
    [Key]
    public Guid Id { get; set; }

    public string? InvoiceNumber { get; set; }

    [Required]
    public required string Name { get; set; }
    public int Hours { get; set; }
    public float CostPerHour { get; set; }

    [Required]
    public Guid UserId { get; set; }
    public required User User { get; set; }

    [Required]
    public Guid CaseId { get; set; }
    public required CourtCase Case { get; set; }
}
