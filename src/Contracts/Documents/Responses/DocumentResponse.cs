namespace Contracts.Documents.Responses;

public record DocumentResponse(
    string CaseNumber,
    string Client,
    IList<FolderResponse> Folders
);

public record FolderResponse(
    Guid Id,
    string Description,
    string ContentType,
    string Version,
    string Date,
    IList<FolderResponse> Folders);
