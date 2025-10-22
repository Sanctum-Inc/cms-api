using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.InvoiceItem.Commands.Delete;
public class DeleteCommandHandler : IRequestHandler<DeleteCommand, ErrorOr<bool>>
{
    private readonly IInvoiceItemService _invoiceItemService;

    public DeleteCommandHandler(IInvoiceItemService invoiceItemService)
    {
        _invoiceItemService = invoiceItemService;
    }

    public async Task<ErrorOr<bool>> Handle(DeleteCommand request, CancellationToken cancellationToken)
    {
        var result = await _invoiceItemService.Delete(request.Id, cancellationToken);

        return result;
    }
}
