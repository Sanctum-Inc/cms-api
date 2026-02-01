using Application.Common.Models;
using Application.Invoice.Commands.SetIsPaid;
using ErrorOr;

namespace Application.Common.Interfaces.Services;

/// <summary>
/// Provides operations for managing invoice items and generating invoice documents.
/// Inherits basic CRUD functionality from <see cref="IBaseService{T}"/>.
/// </summary>
public interface IInvoiceService : IBaseService<InvoiceResult>
{
    /// <summary>
    /// Generates a PDF invoice document for a specific invoice item or invoice record.
    /// </summary>
    /// <param name="id">The unique identifier of the invoice for which the PDF should be generated.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A <see cref="DownloadDocumentResult"/> containing the generated PDF file and related metadata,
    /// such as file name and content type.
    /// </returns>
    /// <remarks>
    /// This method is typically used to allow users to download or view a finalized invoice document.
    /// </remarks>
    Task<ErrorOr<DownloadDocumentResult>> GenerateInvoicePdf(Guid id, CancellationToken cancellationToken);
    Task<string> GetNewInvoiceNumber(CancellationToken cancellationToken);
    Task<ErrorOr<bool>> UpdateIsPaid(SetIsPaidCommand request, CancellationToken cancellationToken);
    Task<ErrorOr<IEnumerable<InvoiceNumbersResult>>> GetInvoiceNumbers(CancellationToken cancellationToken);
}
