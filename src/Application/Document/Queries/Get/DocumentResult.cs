using System.Net.Mime;

namespace Application.Document.Queries.Get;
public record DocumentResult(
    Guid Id,
    string Name,
    string FileName,
    long Size,
    DateTime Created,
    Guid CaseId,
    string ContentType,
    Guid CreatedBy
);
