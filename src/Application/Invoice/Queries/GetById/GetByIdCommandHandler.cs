using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.Invoice.Queries.GetById;
public class GetByIdCommandHandler : IRequestHandler<GetByIdCommand, ErrorOr<InvoiceResult>>
{
    public Task<ErrorOr<InvoiceResult>> Handle(GetByIdCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
