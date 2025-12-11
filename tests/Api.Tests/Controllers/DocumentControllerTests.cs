using System.Net;
using Api.Controllers;
using Application.Common.Models;
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
        _controller = new DocumentController(_mapperMock.Object, _mediatorMock.Object);
    }

    [Fact]
    public async Task Upload_ShouldReturnCreated_WhenSuccessful()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns("test.pdf");
        var name = "My File";
        var caseId = Guid.NewGuid();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<AddCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ErrorOrFactory.From(caseId));

        // Act
        var result = await _controller.Upload(fileMock.Object, name, caseId.ToString());

        // Assert
        var createdResult = result as ObjectResult;
        createdResult.Should().NotBeNull();
        createdResult!.StatusCode.Should().Be((int)HttpStatusCode.Created);
    }

    [Fact]
    public async Task UpdateName_ShouldReturnNoContent_WhenSuccessful()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = new UpdateDocumentRequest("Updated");
        var command = new UpdateCommand(id ,request.FileName);

        _mapperMock
            .Setup(m => m.Map<UpdateCommand>(It.IsAny<UpdateDocumentRequest>()))
            .Returns(command);

        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ErrorOrFactory.From(true));

        // Act
        var result = await _controller.Update(id, request);

        // Assert
        var noContentResult = result as NoContentResult;
        noContentResult.Should().NotBeNull();
        noContentResult!.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task GetStructure_ShouldReturnOk_WithDocuments()
    {
        // Arrange
        var documents = new List<DocumentResult>
        {
            new DocumentResult(
                Guid.NewGuid(),
                "Doc1",
                "file.pdf",
                2651,
                DateTime.UtcNow,
                Guid.NewGuid(),
                "application/pdf",
                Guid.NewGuid())
        };

        IEnumerable<DocumentResponse> documentsResponse = [ new DocumentResponse(
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
            .Setup(m => m.Map<IEnumerable<DocumentResponse>>(It.IsAny<IEnumerable<DocumentResult>>()))
            .Returns(documentsResponse);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        (okResult.Value as IEnumerable<DocumentResponse>).Should().NotBeNull();
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WhenFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var documentResult = new DocumentResult(
            id,
            "Doc1",
            "file.pdf",
            1000,
            DateTime.UtcNow,
            Guid.NewGuid(),
            "application/pdf",
            Guid.NewGuid()
        );

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetByIdCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(documentResult.ToErrorOr()); // ✅ returning ErrorOr<DocumentResult>

        _mapperMock
            .Setup(m => m.Map<DocumentResponse>(It.IsAny<DocumentResult>())) // ✅ correct types
            .Returns(new DocumentResponse(
                documentResult.Id,
                documentResult.Name,
                documentResult.FileName,
                documentResult.Size,
                documentResult.Created,
                documentResult.CaseId,
                documentResult.ContentType,
                documentResult.CreatedBy
            ));

        var controller = new DocumentController(_mapperMock.Object, _mediatorMock.Object);

        // Act
        var result = await controller.GetById(id);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        okResult.Value.Should().BeOfType<DocumentResponse>();

        var response = okResult.Value as DocumentResponse;
        response!.Id.Should().Be(documentResult.Id);
        response.Name.Should().Be(documentResult.Name);
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
        var result = await _controller.Delete(id);

        // Assert
        var noContent = result as NoContentResult;
        noContent.Should().NotBeNull();
        noContent!.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
    }
}
