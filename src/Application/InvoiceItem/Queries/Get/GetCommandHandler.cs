using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.InvoiceItem.Queries.Get;
public class GetCommandHandler : IRequestHandler<GetCommand, ErrorOr<IEnumerable<Common.Models.InvoiceItemResult>>>
{
    private readonly IInvoiceItemService _invoiceItemService;

    public GetCommandHandler(IInvoiceItemService invoiceItemService)
    {
        _invoiceItemService = invoiceItemService;
    }

    public async Task<ErrorOr<IEnumerable<Common.Models.InvoiceItemResult>>> Handle(GetCommand request, CancellationToken cancellationToken)
    {
        var result = await _invoiceItemService.Get(cancellationToken);

        return result;
    }
}
