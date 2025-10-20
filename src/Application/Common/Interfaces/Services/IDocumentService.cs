using Application.Common.Models;
using Application.Document.Queries.Get;
using Application.Document.Queries.GetById;
using Domain.Documents;
using ErrorOr;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Interfaces.Services;

/// <summary>
/// Defines operations for managing document storage, retrieval, and metadata.
/// </summary>
public interface IDocumentService : IBaseService<DocumentResult>
{
    /// <summary>
    /// Downloads the file content of a specific document.
    /// </summary>
    /// <param name="id">The unique identifier of the document.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>
    /// A <see cref="DownloadDocumentResult"/> containing the file stream and metadata if found; otherwise, <c>null</c>.
    /// </returns>
    Task<ErrorOr<DownloadDocumentResult?>> Download(Guid id, CancellationToken cancellationToken = default);
}
