namespace Domain.Invoices;

public enum InvoiceStatus
{
    PENDING,
    SENT,
    PAID,
    OVERDUE,
    CANCELLED,
    PARTIALLY_PAID,
    DRAFT
}
