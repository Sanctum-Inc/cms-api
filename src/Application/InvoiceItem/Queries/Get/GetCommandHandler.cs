using Application.Common.Interfaces.Services;
using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.InvoiceItem.Queries.Get;

public class GetCommandHandler : IRequestHandler<GetCommand, ErrorOr<IEnumerable<InvoiceItemResult>>>
{
    private readonly IInvoiceItemService _invoiceItemService;

    public GetCommandHandler(IInvoiceItemService invoiceItemService)
    {
        _invoiceItemService = invoiceItemService;
    }

    public async Task<ErrorOr<IEnumerable<InvoiceItemResult>>> Handle(GetCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _invoiceItemService.Get(cancellationToken);

        return result.Value.Select(x =>
            {
                return new InvoiceItemResult(
                    x.Id,
                    x.Date,
                    x.Name,
                    x.Hours,
                    x.CostPerHour,
                    x.CostPerHour * x.Hours
                );
            })
            .ToErrorOr();
    }
}
