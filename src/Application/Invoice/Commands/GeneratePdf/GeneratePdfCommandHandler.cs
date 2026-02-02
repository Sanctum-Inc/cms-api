using Application.Common.Interfaces.Services;
using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.Invoice.Commands.GeneratePdf;

public class GeneratePdfCommandHandler : IRequestHandler<GeneratePdfCommand, ErrorOr<DownloadDocumentResult>>
{
    private readonly IInvoiceService _invoiceService;

    public GeneratePdfCommandHandler(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    public async Task<ErrorOr<DownloadDocumentResult>> Handle(GeneratePdfCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _invoiceService.GenerateInvoicePdf(request.Id, cancellationToken);

        return result;
    }
}
