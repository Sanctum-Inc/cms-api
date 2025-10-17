namespace Contracts.Documents.Requests;
public record UpdateDocumentRequest(
    string Id,
    string FileName);