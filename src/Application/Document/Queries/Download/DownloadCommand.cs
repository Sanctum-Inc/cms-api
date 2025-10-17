using ErrorOr;
using MediatR;

namespace Application.Document.Queries.Download;
public record DownloadCommand(
    Guid Id) : IRequest<ErrorOr<DownloadDocumentResult?>>;
