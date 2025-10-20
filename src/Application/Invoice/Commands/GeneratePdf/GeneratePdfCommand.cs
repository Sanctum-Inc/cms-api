using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.Invoice.Commands.GeneratePdf;
public record GeneratePdfCommand(Guid Id) : IRequest<ErrorOr<DownloadDocumentResult>>;
