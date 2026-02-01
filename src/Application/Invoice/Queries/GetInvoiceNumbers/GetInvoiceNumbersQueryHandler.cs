using Application.Common.Interfaces.Services;
using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.Invoice.Queries.GetInvoiceNumbers;
public class GetInvoiceNumbersQueryHandler : IRequestHandler<GetInvoiceNumbersQuery, ErrorOr<IEnumerable<InvoiceNumbersResult>>>
{
    private readonly IInvoiceService _invoiceService;

    public GetInvoiceNumbersQueryHandler(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    public async Task<ErrorOr<IEnumerable<InvoiceNumbersResult>>> Handle(GetInvoiceNumbersQuery request, CancellationToken cancellationToken)
    {
        var result = await _invoiceService.GetInvoiceNumbers(cancellationToken);

        return result;
    }
}
