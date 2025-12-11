using Domain.Invoices;
using ErrorOr;
using MediatR;

namespace Application.Invoice.Commands.SetIsPaid;
public record SetIsPaidCommand(Guid Id, InvoiceStatus isPaid) : IRequest<ErrorOr<bool>>;
