using Application.Document.Queries.Download;
using Application.Document.Queries.Get;
using Application.Document.Queries.GetById;
using ErrorOr;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Interfaces.Services;

/// <summary>
/// Defines operations for managing document storage, retrieval, and metadata.
/// </summary>
public interface IDocumentService
{
    /// <summary>
    /// Uploads a new document with the specified name.
    /// </summary>
    /// <param name="file">The file to upload.</param>
    /// <param name="name">The name to assign to the document.</param>
    /// <param name="caseId">The caseId associated with the document.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>
    /// <c>true</c> if the document was successfully uploaded; otherwise, <c>false</c>.
    /// </returns>
    Task<ErrorOr<bool>> Add(IFormFile file, string name, string caseId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the name of an existing document.
    /// </summary>
    /// <param name="id">The unique identifier of the document to update.</param>
    /// <param name="newName">The new name to assign to the document.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>
    /// <c>true</c> if the update was successful; otherwise, <c>false</c>.
    /// </returns>
    Task<ErrorOr<bool>> Update(Guid id, string newName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the file structure and metadata for all stored documents.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>
    /// A collection of <see cref="GetDocumentResult"/> objects representing all documents.
    /// </returns>
    Task<ErrorOr<IEnumerable<GetDocumentResult?>>> Get(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets metadata and attributes for a specific document.
    /// </summary>
    /// <param name="id">The unique identifier of the document.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>
    /// A <see cref="GetDocumentByIdResult"/> object containing document metadata if found; otherwise, <c>null</c>.
    /// </returns>
    Task<ErrorOr<GetDocumentByIdResult?>> GetById(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads the file content of a specific document.
    /// </summary>
    /// <param name="id">The unique identifier of the document.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>
    /// A <see cref="DownloadDocumentResult"/> containing the file stream and metadata if found; otherwise, <c>null</c>.
    /// </returns>
    Task<ErrorOr<DownloadDocumentResult?>> Download(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a document by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the document to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>
    /// <c>true</c> if the document was successfully deleted; otherwise, <c>false</c>.
    /// </returns>
    Task<ErrorOr<bool>> Delete(Guid id, CancellationToken cancellationToken = default);
}