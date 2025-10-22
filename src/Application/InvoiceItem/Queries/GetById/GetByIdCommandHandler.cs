using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.InvoiceItem.Queries.GetById;
public class GetByIdCommandHandler : IRequestHandler<GetByIdCommand, ErrorOr<Common.Models.InvoiceItemResult?>>
{
    private readonly IInvoiceItemService _invoiceItemService;

    public GetByIdCommandHandler(IInvoiceItemService invoiceItemService)
    {
        _invoiceItemService = invoiceItemService;
    }

    public async Task<ErrorOr<Common.Models.InvoiceItemResult?>> Handle(GetByIdCommand request, CancellationToken cancellationToken)
    {
        var result = await _invoiceItemService.GetById(request.Id, cancellationToken);

        return result;
    }
}
