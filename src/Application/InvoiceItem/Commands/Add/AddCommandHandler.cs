using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.InvoiceItem.Commands.Add;
public class AddCommandHandler : IRequestHandler<AddCommand, ErrorOr<Guid>>
{
    private readonly IInvoiceItemService _invoiceItemService;

    public AddCommandHandler(IInvoiceItemService invoiceItemService)
    {
        _invoiceItemService = invoiceItemService;
    }

    public async Task<ErrorOr<Guid>> Handle(AddCommand request, CancellationToken cancellationToken)
    {
        var result = await _invoiceItemService.Add(request, cancellationToken);

        return result;
    }
}
