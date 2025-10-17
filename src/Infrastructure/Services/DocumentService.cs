using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Common.Interfaces.Session;
using Application.Document.Queries.Download;
using Application.Document.Queries.Get;
using Application.Document.Queries.GetById;
using ErrorOr;
using Infrastructure.Config;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;
public class DocumentService : IDocumentService
{
    private readonly ISessionResolver _sessionResolver;
    private readonly IDocumentRepository _documentRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICourtCaseRepository _courtCaseRepository;
    private readonly string _rootPath;

    public DocumentService(
        ISessionResolver sessionResolver,
        IDocumentRepository documentRepository,
        IOptions<DocumentStorageOptions> options,
        IUserRepository userRepository,
        ICourtCaseRepository courtCaseRepository)
    {
        _sessionResolver = sessionResolver;
        _documentRepository = documentRepository;

        _rootPath = options.Value.RootPath;

        // Ensure directory exists at startup
        if (!Directory.Exists(_rootPath))
        {
            Directory.CreateDirectory(_rootPath);
        }

        _userRepository = userRepository;
        _courtCaseRepository = courtCaseRepository;
    }

    public async Task<ErrorOr<bool>> Add(IFormFile file, string name, string caseId, CancellationToken cancellationToken = default)
    {
        var userId = _sessionResolver.UserId;
        if (string.IsNullOrEmpty(userId))
            return Error.Unauthorized(description: "User is not authenticated.");

        var user = await _userRepository.GetByIdAsync(Guid.Parse(userId), cancellationToken);

        var courtCase = await _courtCaseRepository.GetByCaseIdAsync(Guid.Parse(caseId), Guid.Parse(userId), cancellationToken);
        if (courtCase == null)
            return Error.NotFound(description: "Court case not found for the user.");

        if (file == null || file.Length == 0) 
            return false;

        var fileExtension = Path.GetExtension(file.FileName);
        var safeFileName = $"{Guid.NewGuid()}{fileExtension}";
        var targetPath = Path.Combine(_rootPath, safeFileName);

        try
        {
            await using var stream = new FileStream(targetPath, FileMode.Create);
            await file.CopyToAsync(stream, cancellationToken);

            await _documentRepository.AddAsync(new Domain.Documents.Document()
            {
                Id = Guid.NewGuid(),
                Name = name,
                FileName = safeFileName,
                ContentType = file.ContentType,
                Size = file.Length,
                CreatedBy = _sessionResolver.UserId,
                Created = DateTime.UtcNow,
                UserId = user!.Id,
                User = user,
                CaseId = courtCase.Id,
                Case = courtCase

            }, cancellationToken);
            await _documentRepository.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch
        {
            // optional: log the error here
            return false;
        }
    }

    public async Task<ErrorOr<bool>> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var document = await _documentRepository.GetByIdAndUserIdAsync(id, cancellationToken);

        if (document == null)
            return Error.NotFound("Document.NotFound", "The document with the specified Id was not found");

        await _documentRepository.DeleteAsync(document, cancellationToken);
        await _documentRepository.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<ErrorOr<DownloadDocumentResult?>> Download(Guid id, CancellationToken cancellationToken = default)
    {
        // TODO: Get metadata from DB (e.g. stored file path, name, content type)
        var document = await _documentRepository.GetByIdAsync(id, cancellationToken);

        if (document == null)
            return Error.NotFound("Document.NotFound", "The document with the specified Id was not found");

        var filePath = Path.Combine(_rootPath, document.FileName); // placeholder

        if (!File.Exists(filePath))
            return Error.NotFound("Document.NotFound", "The document with the specified path was not found");

        var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        var contentType = document.ContentType; // detect from metadata or use FileExtensionContentTypeProvider
        var fileName = document.Name;

        return new DownloadDocumentResult(stream, contentType, fileName);
    }

    public async Task<ErrorOr<IEnumerable<GetDocumentResult?>>> Get(CancellationToken cancellationToken = default)
    {
        // Fetch documents from the database
        var documents = await _documentRepository
            .GetAll(cancellationToken);

        // Map to GetDocumentResult
        var results = documents.Select(doc => new GetDocumentResult(
            doc.Id,
            doc.Name,
            doc.FileName,
            doc.Size,
            doc.Created,
            doc.CaseId
        ));

        return results.ToErrorOr()!;
    }

    public async Task<ErrorOr<GetDocumentByIdResult?>> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        // 🗂️ 1. Get metadata from DB
        var document = await _documentRepository.GetByIdAndUserIdAsync(id, cancellationToken);

        if (document is null)
        {
            return Error.NotFound(
                code: "Document.NotFound",
                description: $"Document with ID '{id}' was not found."
            );
        }

        // 🧾 2. Map to result
        var result = new GetDocumentByIdResult(
            document.Id,
            document.Name,
            document.FileName,
            document.ContentType,
            document.Size,
            document.Created,
            document.CaseId
        );

        return result;
    }

    public async Task<ErrorOr<bool>> Update(Guid id, string newName, CancellationToken cancellationToken = default)
    {
        var document = await _documentRepository.GetByIdAsync(id, cancellationToken);

        if (document == null)
            return Error.NotFound("Document.NotFound", "The document with the specified Id was not found");

        document.Name = newName;

        await _documentRepository.UpdateAsync(document, cancellationToken);
        await _documentRepository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
