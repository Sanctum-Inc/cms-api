namespace Contracts.Documents.Responses;
public record GetDocumentResponse(
    Guid Id,
    string Name,
    string FileName,
    long Size,
    DateTime CreatedAt);
