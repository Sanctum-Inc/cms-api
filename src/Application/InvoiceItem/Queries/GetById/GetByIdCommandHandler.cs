using Application.Common.Interfaces.Services;
using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.InvoiceItem.Queries.GetById;

public class GetByIdCommandHandler : IRequestHandler<GetByIdCommand, ErrorOr<InvoiceItemResult?>>
{
    private readonly IInvoiceItemService _invoiceItemService;

    public GetByIdCommandHandler(IInvoiceItemService invoiceItemService)
    {
        _invoiceItemService = invoiceItemService;
    }

    public async Task<ErrorOr<InvoiceItemResult?>> Handle(GetByIdCommand request, CancellationToken cancellationToken)
    {
        var result = await _invoiceItemService.GetById(request.Id, cancellationToken);

        return result;
    }
}
