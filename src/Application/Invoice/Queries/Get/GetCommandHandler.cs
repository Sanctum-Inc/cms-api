using System.Linq;
using Application.Common.Interfaces.Services;
using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.Invoice.Queries.Get;
public class GetCommandHandler : IRequestHandler<GetCommand, ErrorOr<IEnumerable<InvoiceResult>>>
{
    private readonly IInvoiceService _invoiceService;
    public GetCommandHandler(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    public async Task<ErrorOr<IEnumerable<InvoiceResult>>> Handle(GetCommand request, CancellationToken cancellationToken)
    {
        var result = await _invoiceService.Get(cancellationToken);

        return result;
    }
}
