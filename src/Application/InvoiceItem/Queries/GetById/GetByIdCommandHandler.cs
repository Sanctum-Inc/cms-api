using Application.Common.Interfaces.Services;
using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.InvoiceItem.Queries.GetById;

public class GetByIdCommandHandler(IInvoiceItemService invoiceItemService)
    : IRequestHandler<GetByIdCommand, ErrorOr<InvoiceItemResult>>
{
    public async Task<ErrorOr<InvoiceItemResult>> Handle(GetByIdCommand request, CancellationToken cancellationToken)
    {
        var result = await invoiceItemService.GetById(request.Id, cancellationToken);

        return result;
    }
}
