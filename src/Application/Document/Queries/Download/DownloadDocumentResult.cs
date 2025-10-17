namespace Application.Document.Queries.Download;
public record DownloadDocumentResult(Stream Stream, string ContentType, string FileName);
