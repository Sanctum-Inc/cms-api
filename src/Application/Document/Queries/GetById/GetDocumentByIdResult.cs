namespace Application.Document.Queries.GetById;

public record GetDocumentByIdResult(
    Guid Id,
    string Name,
    string FileName,
    string ContentType,
    long Size,
    DateTime CreatedAt,
    Guid CaseId
);
