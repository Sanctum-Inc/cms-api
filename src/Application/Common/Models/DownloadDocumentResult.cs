namespace Application.Common.Models;
public record DownloadDocumentResult(Stream Stream, string ContentType, string FileName);
