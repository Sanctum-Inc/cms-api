using Api.Controllers;
using Application.CourtCase.Commands.Add;
using Application.CourtCase.Commands.Delete;
using Application.CourtCase.Commands.Update;
using Application.CourtCase.Queries.Get;
using Contracts.CourtCases.Requests;
using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using FluentAssertions;

namespace Api.Integration.Tests.Controllers;

public class CourtCaseControllerTests
{
    private readonly Mock<ISender> _mockSender;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CourtCaseController _controller;

    public CourtCaseControllerTests()
    {
        _mockSender = new Mock<ISender>();
        _mockMapper = new Mock<IMapper>();
        _controller = new CourtCaseController(_mockSender.Object, _mockMapper.Object);
    }

    #region Get Tests

    [Fact]
    public async Task Get_ReturnsOkResult_WithCourtCases()
    {
        // Arrange
        var expectedResult = ErrorOrFactory.From(new GetCourtCaseResult());

        _mockSender
            .Setup(s => s.Send(It.IsAny<GetCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.Get();

        // Assert
        Assert.IsType<OkObjectResult>(result);
        _mockSender.Verify(s => s.Send(It.IsAny<GetCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region GetById Tests

    [Fact]
    public void GetById_ReturnsOkResult_WithValidId()
    {
        // Arrange
        string courtCaseId = "550e8400-e29b-41d4-a716-446655440000";

        // Act
        var result = _controller.GetById(courtCaseId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal($"CourtCaseController received ID: {courtCaseId}", okResult.Value);
    }

    [Theory]
    [InlineData("550e8400-e29b-41d4-a716-446655440000")]
    [InlineData("123e4567-e89b-12d3-a456-426614174000")]
    [InlineData("abc12345-def6-7890-ghij-klmnopqrstuv")]
    public void GetById_ReturnsOkResult_WithDifferentIds(string id)
    {
        // Act
        var result = _controller.GetById(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Contains(id, okResult.Value?.ToString());
    }

    #endregion

    #region Create Tests

    [Fact]
    public async Task Create_ReturnsCreated_WithValidRequest()
    {
        // Arrange
        var addRequest = new AddCourtCaseRequest(
            CaseNumber: "CASE-2024-001",
            Location: "Johannesburg High Court",
            Plaintiff: "John Doe",
            Defendant: "Jane Smith",
            Status: "Pending",
            Type: "Civil",
            Outcome: null
        );

        var addCommand = new AddCommand(
            addRequest.CaseNumber,
            addRequest.Location,
            addRequest.Plaintiff,
            addRequest.Defendant,
            addRequest.Status,
            addRequest.Type,
            addRequest.Outcome
        );

        var successResult = ErrorOrFactory.From(true);

        _mockMapper
            .Setup(m => m.Map<AddCommand>(addRequest))
            .Returns(addCommand);

        _mockSender
            .Setup(s => s.Send(It.IsAny<AddCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(successResult);

        // Act
        var result = await _controller.Create(addRequest);

        // Assert
        Assert.IsType<CreatedResult>(result);
        _mockMapper.Verify(m => m.Map<AddCommand>(addRequest), Times.Once);
        _mockSender.Verify(s => s.Send(It.Is<AddCommand>(c =>
            c.CaseNumber == addRequest.CaseNumber &&
            c.Location == addRequest.Location &&
            c.Plaintiff == addRequest.Plaintiff &&
            c.Defendant == addRequest.Defendant
        ), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenCommandFails()
    {
        // Arrange
        var addRequest = new AddCourtCaseRequest(
            CaseNumber: "",
            Location: "Johannesburg High Court",
            Plaintiff: "John Doe",
            Defendant: "Jane Smith",
            Status: "Pending",
            Type: null,
            Outcome: null
        );

        var addCommand = new AddCommand(
            addRequest.CaseNumber,
            addRequest.Location,
            addRequest.Plaintiff,
            addRequest.Defendant,
            addRequest.Status,
            addRequest.Type,
            addRequest.Outcome
        );

        var errorResult = Error.Validation("CaseNumber.Invalid", "Case number is required");

        _mockMapper
            .Setup(m => m.Map<AddCommand>(addRequest))
            .Returns(addCommand);

        _mockSender
            .Setup(s => s.Send(It.IsAny<AddCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(errorResult);

        // Act
        var result = await _controller.Create(addRequest);

        // Assert

        var objectResult = result as ObjectResult;
        // Assert
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be(400); // or your expected code
    }

    [Fact]
    public async Task Create_WithOptionalFields_ReturnsCreatedResult()
    {
        // Arrange
        var addRequest = new AddCourtCaseRequest(
            CaseNumber: "CASE-2024-002",
            Location: "Pretoria High Court",
            Plaintiff: "Alice Johnson",
            Defendant: "Bob Williams",
            Status: "Active",
            Type: null,
            Outcome: null
        );

        var addCommand = new AddCommand(
            addRequest.CaseNumber,
            addRequest.Location,
            addRequest.Plaintiff,
            addRequest.Defendant,
            addRequest.Status,
            addRequest.Type,
            addRequest.Outcome
        );

        var successResult = ErrorOrFactory.From(true);

        _mockMapper
            .Setup(m => m.Map<AddCommand>(addRequest))
            .Returns(addCommand);

        _mockSender
            .Setup(s => s.Send(It.IsAny<AddCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(successResult);

        // Act
        var result = await _controller.Create(addRequest);

        // Assert
        Assert.IsType<CreatedResult>(result);
    }

    #endregion

    #region Update Tests

    [Fact]
    public async Task Update_ReturnsOkResult_WithValidIdFromRoute()
    {
        // Arrange
        string routeId = "550e8400-e29b-41d4-a716-446655440000";

        var updateRequest = new UpdateCourtCaseRequest(
            CaseNumber: "CASE-2024-001",
            Location: "Johannesburg High Court",
            Plaintiff: "John Doe",
            Defendant: "Jane Smith",
            Status: "Ongoing",
            Type: "Civil",
            Outcome: null
        );

        var mappedCommand = new UpdateCommand(
            Id: "some-initial-id", // This will be overridden by route id
            CaseNumber: updateRequest.CaseNumber,
            Location: updateRequest.Location,
            Plaintiff: updateRequest.Plaintiff,
            Defendant: updateRequest.Defendant,
            Status: updateRequest.Status,
            Type: updateRequest.Type,
            Outcome: updateRequest.Outcome
        );

        var successResult = ErrorOrFactory.From(true);

        _mockMapper
            .Setup(m => m.Map<UpdateCommand>(updateRequest))
            .Returns(mappedCommand);

        _mockSender
            .Setup(s => s.Send(It.Is<UpdateCommand>(c => c.Id == routeId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(successResult);

        // Act
        var result = await _controller.Update(routeId, updateRequest);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _mockMapper.Verify(m => m.Map<UpdateCommand>(updateRequest), Times.Once);
        _mockSender.Verify(s => s.Send(
            It.Is<UpdateCommand>(c =>
                c.Id == routeId &&
                c.CaseNumber == updateRequest.CaseNumber &&
                c.Status == updateRequest.Status
            ),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenCourtCaseDoesNotExist()
    {
        // Arrange
        string routeId = "550e8400-e29b-41d4-a716-446655440000";

        var updateRequest = new UpdateCourtCaseRequest(
            CaseNumber: "CASE-2024-999",
            Location: "Johannesburg High Court",
            Plaintiff: "John Doe",
            Defendant: "Jane Smith",
            Status: "Ongoing",
            Type: "Civil",
            Outcome: null
        );

        var mappedCommand = new UpdateCommand(
            Id: "some-initial-id",
            CaseNumber: updateRequest.CaseNumber,
            Location: updateRequest.Location,
            Plaintiff: updateRequest.Plaintiff,
            Defendant: updateRequest.Defendant,
            Status: updateRequest.Status,
            Type: updateRequest.Type,
            Outcome: updateRequest.Outcome
        );

        var errorResult = Error.NotFound("CourtCase.NotFound", "Court case not found");

        _mockMapper
            .Setup(m => m.Map<UpdateCommand>(updateRequest))
            .Returns(mappedCommand);

        _mockSender
            .Setup(s => s.Send(It.IsAny<UpdateCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(errorResult);

        // Act
        var result = await _controller.Update(routeId, updateRequest);

        // Assert

        var objectResult = result as ObjectResult;
        // Assert
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be(404); // or your expected code
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenValidationFails()
    {
        // Arrange
        string routeId = "550e8400-e29b-41d4-a716-446655440000";

        var updateRequest = new UpdateCourtCaseRequest(
            CaseNumber: "",
            Location: "Johannesburg High Court",
            Plaintiff: "John Doe",
            Defendant: "Jane Smith",
            Status: "Ongoing",
            Type: "Civil",
            Outcome: null
        );

        var mappedCommand = new UpdateCommand(
            Id: "some-initial-id",
            CaseNumber: updateRequest.CaseNumber,
            Location: updateRequest.Location,
            Plaintiff: updateRequest.Plaintiff,
            Defendant: updateRequest.Defendant,
            Status: updateRequest.Status,
            Type: updateRequest.Type,
            Outcome: updateRequest.Outcome
        );

        var errorResult = Error.Validation("CaseNumber.Invalid", "Case number cannot be empty");

        _mockMapper
            .Setup(m => m.Map<UpdateCommand>(updateRequest))
            .Returns(mappedCommand);

        _mockSender
            .Setup(s => s.Send(It.IsAny<UpdateCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(errorResult);

        // Act
        var result = await _controller.Update(routeId, updateRequest);

        // Assert

        var objectResult = result as ObjectResult;
        // Assert
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be(400); // or your expected code
    }

    [Theory]
    [InlineData("550e8400-e29b-41d4-a716-446655440000")]
    [InlineData("123e4567-e89b-12d3-a456-426614174000")]
    public async Task Update_UsesRouteIdOverRequestId(string routeId)
    {
        // Arrange
        var updateRequest = new UpdateCourtCaseRequest(
            CaseNumber: "CASE-2024-001",
            Location: "Johannesburg High Court",
            Plaintiff: "John Doe",
            Defendant: "Jane Smith",
            Status: "Ongoing",
            Type: "Civil",
            Outcome: null
        );

        var mappedCommand = new UpdateCommand(
            Id: "different-id",
            CaseNumber: updateRequest.CaseNumber,
            Location: updateRequest.Location,
            Plaintiff: updateRequest.Plaintiff,
            Defendant: updateRequest.Defendant,
            Status: updateRequest.Status,
            Type: updateRequest.Type,
            Outcome: updateRequest.Outcome
        );

        var successResult = ErrorOrFactory.From(true);

        _mockMapper
            .Setup(m => m.Map<UpdateCommand>(updateRequest))
            .Returns(mappedCommand);

        _mockSender
            .Setup(s => s.Send(It.IsAny<UpdateCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(successResult);

        // Act
        var result = await _controller.Update(routeId, updateRequest);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _mockSender.Verify(s => s.Send(
            It.Is<UpdateCommand>(c => c.Id == routeId),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region Delete Tests

    [Fact]
    public async Task Delete_ReturnsNoContentResult_WithValidIdFromRoute()
    {
        // Arrange
        string routeId = "550e8400-e29b-41d4-a716-446655440000";

        var successResult = ErrorOrFactory.From(true);

        _mockSender
            .Setup(s => s.Send(It.Is<DeleteCommand>(c => c.Id == new Guid(routeId)), It.IsAny<CancellationToken>()))
            .ReturnsAsync(successResult);

        // Act
        var result = await _controller.Delete(routeId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _mockSender.Verify(s => s.Send(
            It.Is<DeleteCommand>(c => c.Id == new Guid(routeId)),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenCourtCaseDoesNotExist()
    {
        // Arrange
        string routeId = "550e8400-e29b-41d4-a716-446655440000";

        var errorResult = Error.NotFound("CourtCase.NotFound", "Court case not found");

        _mockSender
            .Setup(s => s.Send(It.IsAny<DeleteCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(errorResult);

        // Act
        var result = await _controller.Delete(routeId);
        var objectResult = result as ObjectResult;
        // Assert
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be(404); // or your expected code
    }

    [Theory]
    [InlineData("550e8400-e29b-41d4-a716-446655440000")]
    [InlineData("123e4567-e89b-12d3-a456-426614174000")]
    public async Task Delete_WithDifferentIds_SendsCorrectCommand(string id)
    {
        // Arrange
        var successResult = ErrorOrFactory.From(true);

        _mockSender
            .Setup(s => s.Send(It.IsAny<DeleteCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(successResult);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _mockSender.Verify(s => s.Send(
            It.Is<DeleteCommand>(c => c.Id == new Guid(id)),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion
}
