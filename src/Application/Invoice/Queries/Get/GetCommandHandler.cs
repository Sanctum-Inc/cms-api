using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.Invoice.Queries.Get;
public class GetCommandHandler : IRequestHandler<GetCommand, ErrorOr<IEnumerable<InvoiceResult>>>
{
    public Task<ErrorOr<IEnumerable<InvoiceResult>>> Handle(GetCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
