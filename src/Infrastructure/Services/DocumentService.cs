using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Common.Interfaces.Session;
using Application.Common.Models;
using Application.Document.Commands.Add;
using Application.Document.Commands.Update;
using Application.Document.Queries.Get;
using Domain.Common.CommonErrors;
using Domain.Documents;
using ErrorOr;
using MapsterMapper;
using MediatR;

namespace Infrastructure.Services;

public class DocumentService : BaseService<Document, DocumentResult, AddCommand, UpdateCommand>, IDocumentService
{
    private const string DocumentsPrefix = "documents";

    private readonly ICourtCaseRepository _courtCaseRepository;
    private readonly IDocumentRepository _documentRepository;
    private readonly IFileStorage _fileStorage;
    private readonly ISessionResolver _sessionResolver;

    public DocumentService(
        ISessionResolver sessionResolver,
        IDocumentRepository documentRepository,
        IFileStorage fileStorage,
        IMapper mapper,
        ICourtCaseRepository courtCaseRepository) : base(documentRepository, mapper, sessionResolver)
    {
        _sessionResolver = sessionResolver;
        _documentRepository = documentRepository;
        _fileStorage = fileStorage;
        _courtCaseRepository = courtCaseRepository;
    }

    public override async Task<ErrorOr<Guid>> Add(IRequest<ErrorOr<Guid>> request, CancellationToken cancellationToken)
    {
        if (request is not AddCommand addCommand)
        {
            return Error.Failure(description: "Invalid request type.");
        }

        var userId = _sessionResolver.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            return Error.Unauthorized(description: "User is not authenticated.");
        }

        var courtCase =
            await _courtCaseRepository.GetByCaseIdAsync(addCommand.CaseId, Guid.Parse(userId), cancellationToken);
        if (courtCase == null)
        {
            return Error.NotFound(description: "Court case not found for the user.");
        }

        if (addCommand.File == null || addCommand.File.Length == 0)
        {
            return Error.Validation(description: "The file is required.");
        }

        var key = BuildStorageKey(addCommand.File.FileName);

        try
        {
            await using var stream = addCommand.File.OpenReadStream();
            await _fileStorage.UploadAsync(key, stream, addCommand.File.ContentType, cancellationToken);
        }
        catch
        {
            await TryDeleteAsync(key, cancellationToken);
            return Error.Failure(description: "Failed to store the document file.");
        }

        addCommand.StorageKey = key;

        try
        {
            var result = await base.Add(request, cancellationToken);

            if (result.IsError)
            {
                await TryDeleteAsync(key, cancellationToken);
            }

            return result;
        }
        catch
        {
            await TryDeleteAsync(key, cancellationToken);
            throw;
        }
    }

    public async Task<ErrorOr<bool>> AddFile(IRequest<ErrorOr<bool>> request, CancellationToken cancellationToken)
    {
        if (request is not AddCommand addCommand)
        {
            return Error.Failure(description: "Invalid request type.");
        }

        var result = await Add(addCommand, cancellationToken);

        if (result.IsError)
        {
            return result.Errors;
        }

        return true;
    }

    public async Task<ErrorOr<DownloadDocumentResult?>> Download(Guid id, CancellationToken cancellationToken = default)
    {
        var document = await _documentRepository.GetByIdAndUserIdAsync(id, cancellationToken);

        if (document == null)
        {
            return Error.NotFound("Document.NotFound", "The document with the specified Id was not found");
        }

        var key = document.StorageKey ?? document.FileName;

        try
        {
            var stream = await _fileStorage.OpenReadAsync(key, cancellationToken);
            return new DownloadDocumentResult(stream, document.ContentType, document.Name);
        }
        catch (FileNotFoundException)
        {
            return Error.NotFound("Document.NotFound", "The document with the specified path was not found");
        }
    }

    public override async Task<ErrorOr<bool>> Delete(Guid id, CancellationToken cancellationToken)
    {
        var document = await _documentRepository.GetByIdAndUserIdAsync(id, cancellationToken);

        var result = await base.Delete(id, cancellationToken);

        if (!result.IsError && document != null)
        {
            await TryDeleteAsync(document.StorageKey ?? document.FileName, cancellationToken);
        }

        return result;
    }

    protected override Guid GetIdFromUpdateCommand(UpdateCommand command)
    {
        return command.Id;
    }

    protected override ErrorOr<Document> MapFromAddCommand(AddCommand command, string? userId = null)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return Error.Unauthorized(description: "User is not authenticated.");
        }

        return new Document
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            FileName = command.File.FileName,
            ContentType = command.File.ContentType,
            Size = command.File.Length,
            StorageKey = command.StorageKey,
            UserId = new Guid(userId),
            CaseId = command.CaseId
        };
    }

    protected override void MapFromUpdateCommand(Document entity, UpdateCommand command)
    {
        entity.Name = command.FileName;
    }

    public async Task<ErrorOr<IEnumerable<DocumentResult>>> GetAllDocuments(CancellationToken cancellationToken)
    {
        var result = await _documentRepository.GetAll(cancellationToken);

        var documents = result as Document[] ?? result.ToArray();
        if (documents.Length == 0)
        {
            return Errors.Document.NotFound;
        }

        var documentResults = documents
            .GroupBy(d => d.CaseId)
            .Select(group =>
            {
                var courtCase = group.First().Case;
                var rootDocuments = group
                    .Where(d => d.ParentId == null)
                    .ToList();

                return new DocumentResult(
                    courtCase.CaseNumber,
                    $"{courtCase.Defendant} vs {courtCase.Plaintiff}",
                    rootDocuments.Select(MapToFolder).ToList()
                );
            })
            .ToList();

        return documentResults;
    }

    private static string BuildStorageKey(string fileName)
    {
        return $"{DocumentsPrefix}/{Guid.NewGuid()}{Path.GetExtension(fileName)}";
    }

    private static FolderResult MapToFolder(Document document)
    {
        return new FolderResult(
            document.Id,
            document.FileName,
            document.ContentType,
            "1.0",
            document.Created.ToString("yyyy-MM-dd"),
            document.Children
                .Select(child => MapToFolder(child))
                .ToList()
        );
    }

    private async Task TryDeleteAsync(string key, CancellationToken cancellationToken)
    {
        try
        {
            await _fileStorage.DeleteAsync(key, cancellationToken);
        }
        catch
        {
        }
    }
}
