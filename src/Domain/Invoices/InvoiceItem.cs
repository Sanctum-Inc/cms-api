using System.ComponentModel.DataAnnotations;
using Domain.Common;
using Domain.CourtCases;
using Domain.Invoices;
using Domain.Users;

namespace Domain.InvoiceItems;

public class InvoiceItem : AuditableEntity
{
    [Key]
    public Guid Id { get; set; }

    public required Guid InvoiceId { get; set; }

    public Invoice? Invoice { get; set; }
    public string? InvoiceNumber { get; set; }

    [Required]
    public required string Name { get; set; }
    public int Hours { get; set; }
    public decimal? CostPerHour { get; set; }
    public decimal? DayFeeAmount { get; set; }

    [Required]
    public Guid UserId { get; set; }
    public User? User { get; set; }

    [Required]
    public Guid CaseId { get; set; }
    public CourtCase? Case { get; set; }
    public bool IsDayFee { get; set; }
}
