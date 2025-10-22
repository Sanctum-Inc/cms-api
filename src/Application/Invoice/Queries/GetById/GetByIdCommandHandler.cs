using Application.Common.Interfaces.Services;
using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.Invoice.Queries.GetById;
public class GetByIdCommandHandler : IRequestHandler<GetByIdCommand, ErrorOr<InvoiceResult>>
{
    private readonly IInvoiceService _invoiceService;
    public GetByIdCommandHandler(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    public async Task<ErrorOr<InvoiceResult>> Handle(GetByIdCommand request, CancellationToken cancellationToken)
    {
        var result = await _invoiceService.GetById(request.Id, cancellationToken);

        return result;
    }
}
