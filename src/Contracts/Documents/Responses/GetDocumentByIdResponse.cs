namespace Contracts.Documents.Responses;

public record GetDocumentByIdResponse(
    Guid Id,
    string Name,
    string FileName,
    string ContentType,
    long Size,
    DateTime CreatedAt,
    Guid CaseId);
