using System.Xml.Linq;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Common.Interfaces.Session;
using Application.Common.Models;
using Application.Document.Commands.Add;
using Application.Document.Commands.Update;
using Application.Document.Queries.Get;
using Application.Document.Queries.GetById;
using Domain.CourtCases;
using Domain.Documents;
using Domain.Users;
using ErrorOr;
using Infrastructure.Config;
using Infrastructure.Services.Base;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;
public class DocumentService : BaseService<Document, DocumentResult, AddCommand, UpdateCommand>, IDocumentService
{
    private readonly ISessionResolver _sessionResolver;
    private readonly IDocumentRepository _documentRepository;
    private readonly ICourtCaseRepository _courtCaseRepository;
    private readonly string _rootPath;

    public DocumentService(
        ISessionResolver sessionResolver,
        IDocumentRepository documentRepository,
        IOptions<DocumentStorageOptions> options,
        IMapper mapper,
        ICourtCaseRepository courtCaseRepository) : base(documentRepository, mapper, sessionResolver)
    {
        _sessionResolver = sessionResolver;
        _documentRepository = documentRepository;

        _rootPath = options.Value.RootPath;

        // Ensure directory exists at startup
        if (!Directory.Exists(_rootPath))
        {
            Directory.CreateDirectory(_rootPath);
        }

        _courtCaseRepository = courtCaseRepository;
    }

    public async Task<ErrorOr<bool>> AddFile(IRequest<ErrorOr<bool>> request, CancellationToken cancellationToken)
    {
        if (request is not AddCommand addCommand)
            return Error.Failure(description: "Invalid request type.");

        var userId = _sessionResolver.UserId;
        if (string.IsNullOrEmpty(userId))
            return Error.Unauthorized(description: "User is not authenticated.");

        var courtCase = await _courtCaseRepository.GetByCaseIdAsync(addCommand.CaseId, Guid.Parse(userId), cancellationToken);
        if (courtCase == null)
            return Error.NotFound(description: "Court case not found for the user.");

        if (addCommand.File == null || addCommand.File.Length == 0)
            return false;

        var fileExtension = Path.GetExtension(addCommand.File.FileName);
        var safeFileName = $"{Guid.NewGuid()}{fileExtension}";
        var targetPath = Path.Combine(_rootPath, safeFileName);

        try
        {
            await using var stream = new FileStream(targetPath, FileMode.Create);
            await addCommand.File.CopyToAsync(stream, cancellationToken);

            await Add(addCommand, cancellationToken);

            return true;
        }
        catch
        {
            // optional: log the error here
            return false;
        }
    }

    public async Task<ErrorOr<DownloadDocumentResult?>> Download(Guid id, CancellationToken cancellationToken = default)
    {
        var document = await _documentRepository.GetByIdAndUserIdAsync(id, cancellationToken);

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

    protected override Guid GetIdFromUpdateCommand(UpdateCommand command)
    {
        return command.Id;
    }

    protected override ErrorOr<Document> MapFromAddCommand(AddCommand command, string? userId = null)
    {
        if (string.IsNullOrEmpty(userId))
            return Error.Unauthorized(description: "User is not authenticated.");

        return new Domain.Documents.Document()
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            FileName = command.File.FileName,
            ContentType = command.File.ContentType,
            Size = command.File.Length,
            UserId = new Guid(userId),
            CaseId = command.CaseId,
        };
    }

    protected override void MapFromUpdateCommand(Document entity, UpdateCommand command)
    {
        entity.Name = command.FileName;
    }
}
