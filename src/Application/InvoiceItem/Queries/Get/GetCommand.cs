using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.InvoiceItem.Queries.Get;

public record GetCommand : IRequest<ErrorOr<IEnumerable<InvoiceItemResult>>>;
