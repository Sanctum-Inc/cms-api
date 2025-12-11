namespace Contracts.InvoiceItem.Responses;
public record DownloadDocumentResponse(Stream Stream, string ContentType, string FileName);
