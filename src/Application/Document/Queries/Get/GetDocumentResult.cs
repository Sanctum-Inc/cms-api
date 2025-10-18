using System.Net.Mime;

namespace Application.Document.Queries.Get;
public record GetDocumentResult(
    Guid Id,
    string Name,
    string FileName,
    long Size,
    DateTime CreatedAt,
    Guid CaseId,
    string ContentType,
    Guid CreatedBy
);
