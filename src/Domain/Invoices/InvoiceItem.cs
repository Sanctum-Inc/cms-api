using Domain.Common;
using Domain.Invoices;
using Domain.Users;

namespace Domain.InvoiceItems;

public class InvoiceItem : AuditableEntity
{
    public Guid Id { get; set; }

    public required Guid InvoiceId { get; set; }
    public Invoice? Invoice { get; set; }

    public required string Name { get; set; }
    public int Hours { get; set; }
    public decimal? CostPerHour { get; set; }
    public decimal? DayFeeAmount { get; set; }
    public bool IsDayFee { get; set; }

    public required Guid UserId { get; set; }
    public User? User { get; set; }
}
