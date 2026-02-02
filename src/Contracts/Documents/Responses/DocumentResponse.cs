namespace Contracts.Documents.Responses;

public record DocumentResponse(
    Guid Id,
    string Name,
    string FileName,
    long Size,
    DateTime Created,
    Guid CaseId,
    string ContentType,
    Guid CreatedBy
);
