using Domain.Invoices;

namespace Contracts.Invoice.Requests;

public record UpdateInvoiceStatusRequest(InvoiceStatus IsPaid);
