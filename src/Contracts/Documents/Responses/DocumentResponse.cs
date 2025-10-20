namespace Contracts.Documents.Responses;
public record DocumentResponse(
    Guid Id,
    string Name,
    string FileName,
    long Size,
    DateTime CreatedAt,
    Guid CaseId,
    string ContentType,
    Guid CreatedBy);
