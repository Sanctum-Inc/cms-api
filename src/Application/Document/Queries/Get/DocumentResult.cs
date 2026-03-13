namespace Application.Document.Queries.Get;

public record DocumentResult(
    string CaseNumber,
    string Client,
    IList<FolderResult> Folders
);

public record FolderResult(
    Guid Id,
    string Description,
    string ContentType,
    string Version,
    string Date,
    IList<FolderResult> Folders);
