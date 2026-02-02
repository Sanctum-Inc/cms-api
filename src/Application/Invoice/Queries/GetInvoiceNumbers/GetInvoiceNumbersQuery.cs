using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.Invoice.Queries.GetInvoiceNumbers;

public record GetInvoiceNumbersQuery : IRequest<ErrorOr<IEnumerable<InvoiceNumbersResult>>>;
