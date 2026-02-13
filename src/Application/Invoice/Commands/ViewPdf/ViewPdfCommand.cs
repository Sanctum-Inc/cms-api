using Application.Common.Models;
using MediatR;
using ErrorOr;

namespace Application.Invoice.Commands.ViewPDF;

public record ViewPdfCommand(
    Guid Id,
    long Expiry,
    string Signature,
    Guid firmId) :IRequest<ErrorOr<DownloadDocumentResult>>;
