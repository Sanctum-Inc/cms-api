using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Common.Interfaces.Session;
using Application.Document.Commands.Add;
using Domain.CourtCases;
using Domain.Documents;
using ErrorOr;
using FluentAssertions;
using Infrastructure.Services;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Api.Tests.Documents;

public class DocumentServiceTests
{
    private readonly Guid _caseId = Guid.NewGuid();
    private readonly Mock<ICourtCaseRepository> _courtCaseRepositoryMock = new();
    private readonly Mock<IDocumentRepository> _documentRepositoryMock = new();
    private readonly Mock<IFileStorage> _fileStorageMock = new();
    private readonly DocumentService _service;
    private readonly Mock<ISessionResolver> _sessionResolverMock = new();
    private readonly Guid _userId = Guid.NewGuid();

    public DocumentServiceTests()
    {
        _sessionResolverMock.Setup(s => s.UserId).Returns(_userId.ToString());

        _documentRepositoryMock
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _courtCaseRepositoryMock
            .Setup(r => r.GetByCaseIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateCourtCase());

        _service = new DocumentService(
            _sessionResolverMock.Object,
            _documentRepositoryMock.Object,
            _fileStorageMock.Object,
            new Mock<IMapper>().Object,
            _courtCaseRepositoryMock.Object);
    }

    [Fact]
    public async Task Add_ShouldUploadFile_AndPersistStorageKey()
    {
        // Arrange
        Document? captured = null;
        string? uploadedKey = null;
        string? uploadedContentType = null;

        _fileStorageMock
            .Setup(s => s.UploadAsync(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, Stream, string, CancellationToken>((key, _, contentType, _) =>
            {
                uploadedKey = key;
                uploadedContentType = contentType;
            })
            .Returns(Task.CompletedTask);

        _documentRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Document>(), It.IsAny<CancellationToken>()))
            .Callback<Document, CancellationToken>((document, _) => captured = document)
            .Returns(Task.CompletedTask);

        var command = CreateCommand();

        // Act
        var result = await _service.Add(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeEmpty();
        uploadedKey.Should().StartWith("documents/").And.EndWith(".pdf");
        uploadedContentType.Should().Be("application/pdf");
        captured.Should().NotBeNull();
        captured!.StorageKey.Should().Be(uploadedKey);
        captured.FileName.Should().Be("case-file.pdf");
        captured.Size.Should().Be(command.File.Length);
        _documentRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Add_ShouldReturnUnauthorized_WhenUserIsNotAuthenticated()
    {
        // Arrange
        _sessionResolverMock.Setup(s => s.UserId).Returns((string?)null);

        // Act
        var result = await _service.Add(CreateCommand(), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unauthorized);
        _fileStorageMock.Verify(
            s => s.UploadAsync(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>(),
                It.IsAny<CancellationToken>()), Times.Never);
        _documentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Document>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Add_ShouldReturnNotFound_WhenCourtCaseDoesNotExist()
    {
        // Arrange
        _courtCaseRepositoryMock
            .Setup(r => r.GetByCaseIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((CourtCase?)null);

        // Act
        var result = await _service.Add(CreateCommand(), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        _fileStorageMock.Verify(
            s => s.UploadAsync(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>(),
                It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Add_ShouldReturnFailure_AndNotPersist_WhenUploadFails()
    {
        // Arrange
        _fileStorageMock
            .Setup(s => s.UploadAsync(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new IOException("storage unavailable"));

        // Act
        var result = await _service.Add(CreateCommand(), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Failure);
        _documentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Document>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Download_ShouldReturnStream_UsingStorageKey()
    {
        // Arrange
        var document = CreateDocument("documents/stored.pdf");
        var content = new byte[] { 1, 2, 3 };

        _documentRepositoryMock
            .Setup(r => r.GetByIdAndUserIdAsync(document.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(document);
        _fileStorageMock
            .Setup(s => s.OpenReadAsync("documents/stored.pdf", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new MemoryStream(content));

        // Act
        var result = await _service.Download(document.Id, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value!.ContentType.Should().Be("application/pdf");
        result.Value.FileName.Should().Be("Case File");

        await using var stream = result.Value.Stream;
        using var buffer = new MemoryStream();
        await stream.CopyToAsync(buffer);
        buffer.ToArray().Should().Equal(content);
    }

    [Fact]
    public async Task Download_ShouldFallBackToFileName_WhenStorageKeyIsNull()
    {
        // Arrange
        var document = CreateDocument(null);

        _documentRepositoryMock
            .Setup(r => r.GetByIdAndUserIdAsync(document.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(document);
        _fileStorageMock
            .Setup(s => s.OpenReadAsync("case-file.pdf", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new MemoryStream([1]));

        // Act
        var result = await _service.Download(document.Id, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        _fileStorageMock.Verify(s => s.OpenReadAsync("case-file.pdf", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Download_ShouldReturnNotFound_WhenFileIsMissingFromStorage()
    {
        // Arrange
        var document = CreateDocument("documents/stored.pdf");

        _documentRepositoryMock
            .Setup(r => r.GetByIdAndUserIdAsync(document.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(document);
        _fileStorageMock
            .Setup(s => s.OpenReadAsync("documents/stored.pdf", It.IsAny<CancellationToken>()))
            .ThrowsAsync(new FileNotFoundException());

        // Act
        var result = await _service.Download(document.Id, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task Download_ShouldReturnNotFound_WhenDocumentDoesNotExist()
    {
        // Arrange
        _documentRepositoryMock
            .Setup(r => r.GetByIdAndUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Document?)null);

        // Act
        var result = await _service.Download(Guid.NewGuid(), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task Delete_ShouldRemoveStoredObject_AfterDatabaseDelete()
    {
        // Arrange
        var document = CreateDocument("documents/stored.pdf");

        _documentRepositoryMock
            .Setup(r => r.GetByIdAndUserIdAsync(document.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(document);
        _documentRepositoryMock
            .Setup(r => r.DeleteAsync(It.IsAny<Document>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.Delete(document.Id, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeTrue();
        _fileStorageMock.Verify(s => s.DeleteAsync("documents/stored.pdf", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Delete_ShouldStillReturnTrue_WhenStorageDeleteFails()
    {
        // Arrange
        var document = CreateDocument("documents/stored.pdf");

        _documentRepositoryMock
            .Setup(r => r.GetByIdAndUserIdAsync(document.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(document);
        _documentRepositoryMock
            .Setup(r => r.DeleteAsync(It.IsAny<Document>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _fileStorageMock
            .Setup(s => s.DeleteAsync("documents/stored.pdf", It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Amazon.S3.AmazonS3Exception("boom"));

        // Act
        var result = await _service.Delete(document.Id, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeTrue();
    }

    private AddCommand CreateCommand()
    {
        var content = new byte[] { 1, 2, 3, 4, 5 };
        var file = new FormFile(new MemoryStream(content), 0, content.Length, "file", "case-file.pdf")
        {
            Headers = new HeaderDictionary { { "Content-Type", "application/pdf" } }
        };

        return new AddCommand(file, "Case File", _caseId);
    }

    private static CourtCase CreateCourtCase()
    {
        return new CourtCase
        {
            CaseNumber = "CASE-001",
            Location = "Johannesburg",
            Plaintiff = "Plaintiff",
            Defendant = "Defendant",
            Status = (CourtCaseStatus)0,
            Type = (CourtCaseTypes)0,
            Outcome = (CourtCaseOutcomes)0,
            IsPaid = false,
            UserId = Guid.NewGuid()
        };
    }

    private static Document CreateDocument(string? storageKey)
    {
        return new Document
        {
            Id = Guid.NewGuid(),
            Name = "Case File",
            FileName = "case-file.pdf",
            StorageKey = storageKey,
            ContentType = "application/pdf",
            Size = 5,
            UserId = Guid.NewGuid(),
            CaseId = Guid.NewGuid()
        };
    }
}
