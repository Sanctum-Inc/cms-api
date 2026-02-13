using Application.Common.Interfaces.Services;
using Application.Common.Models;
using MediatR;
using ErrorOr;

namespace Application.Invoice.Commands.ViewPDF;

public class ViewPdfCommandHandler : IRequestHandler<ViewPdfCommand, ErrorOr<DownloadDocumentResult>>
{
    private readonly IInvoiceService _invoiceService;

    public ViewPdfCommandHandler(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    public async Task<ErrorOr<DownloadDocumentResult>> Handle(ViewPdfCommand request, CancellationToken cancellationToken)
    {
        var result = await _invoiceService.ViewPdf(request.Id, request.Expiry, request.Signature, request.firmId, cancellationToken);

        return result;
    }
}
