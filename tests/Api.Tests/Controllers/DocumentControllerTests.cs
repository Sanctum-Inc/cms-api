using System.Net;
using Api.Controllers;
using Application.Document.Commands.Add;
using Application.Document.Commands.Delete;
using Application.Document.Commands.Update;
using Application.Document.Queries.Download;
using Application.Document.Queries.Get;
using Application.Document.Queries.GetById;
using Contracts.Documents.Requests;
using Contracts.Documents.Responses;
using ErrorOr;
using FluentAssertions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Api.Tests.Controllers;

public class DocumentControllerTests
{
    private readonly Mock<ISender> _mediatorMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly DocumentController _controller;

    public DocumentControllerTests()
    {
        _mediatorMock = new Mock<ISender>();
        _mapperMock = new Mock<IMapper>();
        _controller = new DocumentController(_mediatorMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Upload_ShouldReturnCreated_WhenSuccessful()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns("test.pdf");
        var name = "My File";
        var caseId = Guid.NewGuid().ToString();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<AddCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ErrorOrFactory.From(true));

        // Act
        var result = await _controller.Upload(fileMock.Object, name, caseId);

        // Assert
        var createdResult = result as ObjectResult;
        createdResult.Should().NotBeNull();
        createdResult!.StatusCode.Should().Be((int)HttpStatusCode.Created);
    }

    [Fact]
    public async Task UpdateName_ShouldReturnNoContent_WhenSuccessful()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var request = new UpdateDocumentRequest("Updated");
        var command = new UpdateCommand(new Guid(id) ,request.FileName);

        _mapperMock
            .Setup(m => m.Map<UpdateCommand>(It.IsAny<UpdateDocumentRequest>()))
            .Returns(command);

        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ErrorOrFactory.From(true));

        // Act
        var result = await _controller.UpdateName(id, request);

        // Assert
        var noContentResult = result as NoContentResult;
        noContentResult.Should().NotBeNull();
        noContentResult!.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task GetStructure_ShouldReturnOk_WithDocuments()
    {
        // Arrange
        var documents = new List<GetDocumentResult>
        {
            new GetDocumentResult(
                Guid.NewGuid(),
                "Doc1",
                "file.pdf",
                2651,
                DateTime.UtcNow,
                Guid.NewGuid(),
                "application/pdf",
                Guid.NewGuid())
        };

        IEnumerable<GetDocumentResponse> documentsResponse = [ new GetDocumentResponse(
                Guid.NewGuid(),
                "Doc1",
                "file.pdf",
                2651,
                DateTime.UtcNow,
                Guid.NewGuid(),
                "application/pdf",
                Guid.NewGuid())
        ];

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(documents);

        _mapperMock
            .Setup(m => m.Map<IEnumerable<GetDocumentResponse>>(It.IsAny<IEnumerable<GetDocumentResult>>()))
            .Returns(documentsResponse);

        // Act
        var result = await _controller.GetStructure();

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        (okResult.Value as IEnumerable<GetDocumentResponse>).Should().NotBeNull();
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WhenFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var documentResult = new GetDocumentByIdResult(id, "Doc1", "file.pdf", "application/pdf", 1000, DateTime.UtcNow, Guid.NewGuid());

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetByIdCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(documentResult);

        _mapperMock
            .Setup(m => m.Map<GetDocumentByIdResponse>(It.IsAny<GetDocumentByIdResult>()))
            .Returns(new GetDocumentByIdResponse(documentResult.Id, documentResult.Name, documentResult.FileName, documentResult.ContentType, documentResult.Size, documentResult.CreatedAt, documentResult.CaseId));

        // Act
        var result = await _controller.GetById(id.ToString());

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        okResult.Value.Should().BeOfType<GetDocumentByIdResponse>();
    }

    [Fact]
    public async Task Download_ShouldReturnFile_WhenFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var memoryStream = new MemoryStream();
        var downloadResult = new DownloadDocumentResult(memoryStream, "application/pdf", "file.pdf");

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DownloadCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(downloadResult);

        // Act
        var result = await _controller.Download(id.ToString());

        // Assert
        result.Should().BeOfType<FileStreamResult>();
        var fileResult = result as FileStreamResult;
        fileResult!.ContentType.Should().Be("application/pdf");
        fileResult.FileDownloadName.Should().Be("file.pdf");
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent_WhenSuccessful()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ErrorOrFactory.From(true));

        // Act
        var result = await _controller.Delete(id.ToString());

        // Assert
        var noContent = result as NoContentResult;
        noContent.Should().NotBeNull();
        noContent!.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
    }
}
