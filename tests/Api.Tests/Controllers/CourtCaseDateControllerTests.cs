using Api.Controllers;
using Application.Common.Models;
using Application.CourtCaseDates.Commands.Add;
using Application.CourtCaseDates.Commands.Delete;
using Application.CourtCaseDates.Commands.Update;
using Application.CourtCaseDates.Queries.Get;
using Application.CourtCaseDates.Queries.GetById;
using Contracts.CourtCaseDates.Requests;
using Contracts.CourtCaseDates.Responses;
using ErrorOr;
using FluentAssertions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Api.Tests.Controllers;

public class CourtCaseDateControllerTests
{
    private readonly Mock<ISender> _mockSender;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CourtCaseDateController _controller;

    public CourtCaseDateControllerTests()
    {
        _mockSender = new Mock<ISender>(MockBehavior.Strict);
        _mockMapper = new Mock<IMapper>(MockBehavior.Strict);
        _controller = new CourtCaseDateController(_mockMapper.Object, _mockSender.Object);
    }

    [Fact]
    public async Task Get_Should_ReturnOk_WithCourtCaseDates()
    {
        // Arrange
        var expected = new List<CourtCaseDateResult> { new CourtCaseDateResult() };
        var expectedResponse = new List<CourtCaseDatesResponse> { new CourtCaseDatesResponse() };

        _mockSender
            .Setup(s => s.Send(It.IsAny<GetCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        _mockMapper
            .Setup(m => m.Map<IEnumerable<CourtCaseDatesResponse>>(It.IsAny<IEnumerable<CourtCaseDateResult>>()))
            .Returns(expectedResponse);

        // Act
        var result = await _controller.Get();

        // Assert
        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(expectedResponse);

        _mockSender.Verify(s => s.Send(It.IsAny<GetCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockMapper.Verify(m => m.Map<IEnumerable<CourtCaseDatesResponse>>(It.IsAny<IEnumerable<CourtCaseDateResult>>()), Times.Once);
    }

    [Fact]
    public async Task GetById_Should_ReturnOk_WhenFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var resultModel = new CourtCaseDateResult();
        var responseModel = new CourtCaseDatesResponse();

        _mockSender
            .Setup(s => s.Send(It.Is<GetByIdCommand>(c => c.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(resultModel);

        _mockMapper
            .Setup(m => m.Map<CourtCaseDatesResponse>(It.IsAny<CourtCaseDateResult>()))
            .Returns(responseModel);

        // Act
        var result = await _controller.GetById(id.ToString());

        // Assert
        result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(responseModel);
    }

    [Fact]
    public async Task Create_Should_ReturnCreated_WhenSuccessful()
    {
        // Arrange
        var request = new AddCourtCaseDateRequest("2025-10-31", "Hearing", Guid.NewGuid());
        var command = new AddCommand(request.Date, request.Title, request.CaseId);

        _mockMapper.Setup(m => m.Map<AddCommand>(request)).Returns(command);
        _mockSender.Setup(s => s.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ErrorOrFactory.From(true));

        // Act
        var result = await _controller.Create(request);

        // Assert
        result.Should().BeOfType<CreatedResult>();
        _mockMapper.Verify(m => m.Map<AddCommand>(request), Times.Once);
        _mockSender.Verify(s => s.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Create_Should_ReturnBadRequest_OnValidationError()
    {
        // Arrange
        var request = new AddCourtCaseDateRequest("", "", Guid.Empty);
        var command = new AddCommand(request.Date, request.Title, request.CaseId);
        var error = Error.Validation("CourtCaseDate.Invalid", "Invalid create request");

        _mockMapper.Setup(m => m.Map<AddCommand>(request)).Returns(command);
        _mockSender.Setup(s => s.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(error);

        // Act
        var result = await _controller.Create(request);

        // Assert
        result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task Update_Should_ReturnNoContent_WhenSuccessful()
    {
        // Arrange
        var routeId = Guid.NewGuid();
        var request = new UpdateCourtCaseDateRequest();
        var command = new UpdateCommand(routeId, "2025-11-01", "Updated Title", Guid.NewGuid());

        _mockMapper.Setup(m => m.Map<UpdateCommand>(request))
            .Returns(command with { Id = Guid.NewGuid() });
        _mockSender.Setup(s => s.Send(It.Is<UpdateCommand>(c => c.Id == routeId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ErrorOrFactory.From(true));

        // Act
        var result = await _controller.Update(routeId.ToString(), request);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _mockSender.Verify(s => s.Send(It.Is<UpdateCommand>(c => c.Id == routeId), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Update_Should_ReturnNotFound_WhenNotFound()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var request = new UpdateCourtCaseDateRequest();
        var mappedCommand = new UpdateCommand(Guid.NewGuid(), "2025-10-31", "Hearing", Guid.NewGuid());
        var error = Error.NotFound("CourtCaseDate.NotFound", "Court case date not found");

        _mockMapper.Setup(m => m.Map<UpdateCommand>(request)).Returns(mappedCommand);
        _mockSender.Setup(s => s.Send(It.IsAny<UpdateCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(error);

        // Act
        var result = await _controller.Update(id, request);

        // Assert
        result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task Delete_Should_ReturnNoContent_WhenSuccessful()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockSender.Setup(s => s.Send(It.Is<DeleteCommand>(c => c.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ErrorOrFactory.From(true));

        // Act
        var result = await _controller.Delete(id.ToString());

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _mockSender.Verify(s => s.Send(It.Is<DeleteCommand>(c => c.Id == id), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Delete_Should_ReturnNotFound_WhenNotFound()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var error = Error.NotFound("CourtCaseDate.NotFound", "Court case date not found");
        _mockSender.Setup(s => s.Send(It.IsAny<DeleteCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(error);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(404);
    }
}


