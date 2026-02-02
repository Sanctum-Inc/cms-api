using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.Invoice.Queries.Get;

public record GetCommand : IRequest<ErrorOr<IEnumerable<InvoiceResult>>>;
