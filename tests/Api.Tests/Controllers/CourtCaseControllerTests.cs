using Api.Controllers;
using Application.Common.Models;
using Application.CourtCase.Commands.Add;
using Application.CourtCase.Commands.Delete;
using Application.CourtCase.Commands.Update;
using Application.CourtCase.Queries.Get;
using Contracts.CourtCases.Requests;
using Contracts.CourtCases.Responses;
using Domain.CourtCases;
using ErrorOr;
using FluentAssertions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Api.Tests.Controllers;

public class CourtCaseControllerTests
{
    private readonly CourtCaseController _controller;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ISender> _mockSender;

    public CourtCaseControllerTests()
    {
        _mockSender = new Mock<ISender>(MockBehavior.Strict);
        _mockMapper = new Mock<IMapper>(MockBehavior.Strict);
        _controller = new CourtCaseController(_mockMapper.Object, _mockSender.Object);
    }

    #region Get Tests

    [Fact]
    public async Task Get_Should_ReturnOk_WithCourtCases()
    {
        // Arrange
        var expectedCases = new List<CourtCaseResult>
        {
            new()
            {
                Id = Guid.NewGuid(),
                CaseNumber = "CASE-2024-001",
                Location = "Johannesburg High Court",
                Plaintiff = "John Doe",
                Defendant = "Jane Smith",
                Status = CourtCaseStatus.Draft,
                Type = CourtCaseTypes.Family,
                Outcome = CourtCaseOutcomes.NotGuilty
            }
        };

        var expectedResponse = expectedCases.Select(c => new CourtCaseResponse
        (
            CaseNumber: c.CaseNumber,
            Location: c.Location,
            Plaintiff: c.Plaintiff,
            Status: c.Status,
            Type: c.Type,
            Id: c.Id,
            NextDate: ""));

        _mockSender
            .Setup(s => s.Send(It.IsAny<GetQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedCases);

        _mockMapper
            .Setup(m => m.Map<IEnumerable<CourtCaseResponse>>(It.IsAny<IEnumerable<CourtCaseResult>>()))
            .Returns((IEnumerable<CourtCaseResult> source) => expectedResponse);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(expectedResponse);

        _mockSender.Verify(s => s.Send(It.IsAny<GetQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockMapper.Verify(m => m.Map<IEnumerable<CourtCaseResponse>>(It.IsAny<IEnumerable<CourtCaseResult>>()),
            Times.Once);
    }

    #endregion

    #region Create Tests

    [Fact]
    public async Task Create_Should_ReturnCreated_WhenSuccessful()
    {
        // Arrange
        var request = new AddCourtCaseRequest(
            "CASE-2024-001",
            "Johannesburg High Court",
            "John Doe",
            "Jane Smith",
            CourtCaseStatus.Draft,
            CourtCaseTypes.Family,
            CourtCaseOutcomes.NotGuilty
        );

        var command = new AddCommand(
            request.CaseNumber,
            request.Location,
            request.Plaintiff,
            request.Defendant,
            request.Status,
            request.Type,
            request.Outcome
        );

        _mockMapper.Setup(m => m.Map<AddCommand>(request)).Returns(command);
        _mockSender.Setup(s => s.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ErrorOrFactory.From(Guid.NewGuid()));

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
        var request = new AddCourtCaseRequest(
            "",
            "Johannesburg High Court",
            "John Doe",
            "Jane Smith",
            CourtCaseStatus.Draft,
            CourtCaseTypes.Family,
            CourtCaseOutcomes.NotLiable
        );

        var command = new AddCommand(
            request.CaseNumber,
            request.Location,
            request.Plaintiff,
            request.Defendant,
            request.Status,
            request.Type,
            request.Outcome
        );

        var error = Error.Validation("CaseNumber.Invalid", "Case number is required");

        _mockMapper.Setup(m => m.Map<AddCommand>(request)).Returns(command);
        _mockSender.Setup(s => s.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(error);

        // Act
        var result = await _controller.Create(request);

        // Assert
        result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(400);
    }

    #endregion

    #region Update Tests

    [Fact]
    public async Task Update_Should_ReturnNoContent_WhenSuccessful()
    {
        // Arrange
        var routeId = Guid.NewGuid();
        var request = new UpdateCourtCaseRequest(
            "CASE-2024-001",
            "Johannesburg High Court",
            "John Doe",
            "Jane Smith",
            CourtCaseStatus.Draft,
            CourtCaseTypes.Family,
            CourtCaseOutcomes.Settled
        );

        var command = new UpdateCommand(
            routeId,
            request.CaseNumber,
            request.Location,
            request.Plaintiff,
            request.Defendant,
            request.Status,
            request.Type,
            request.Outcome
        );

        _mockMapper.Setup(m => m.Map<UpdateCommand>(request))
            .Returns(command with { Id = Guid.NewGuid() }); // simulate mapper returning some Id
        _mockSender.Setup(s => s.Send(It.Is<UpdateCommand>(c => c.Id == routeId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ErrorOrFactory.From(true));

        // Act
        var result = await _controller.Update(routeId, request);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _mockSender.Verify(s => s.Send(It.Is<UpdateCommand>(c => c.Id == routeId), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Update_Should_ReturnNotFound_WhenNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = new UpdateCourtCaseRequest("CASE-404", "Pretoria", "A", "B", CourtCaseStatus.Draft,
            CourtCaseTypes.Family, CourtCaseOutcomes.Guilty);
        var mappedCommand = new UpdateCommand(Guid.NewGuid(), request.CaseNumber, request.Location, request.Plaintiff,
            request.Defendant, request.Status, request.Type, request.Outcome);
        var error = Error.NotFound("CourtCase.NotFound", "Court case not found");

        _mockMapper.Setup(m => m.Map<UpdateCommand>(request)).Returns(mappedCommand);
        _mockSender.Setup(s => s.Send(It.IsAny<UpdateCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(error);

        // Act
        var result = await _controller.Update(id, request);

        // Assert
        result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(404);
    }

    #endregion

    #region Delete Tests

    [Fact]
    public async Task Delete_Should_ReturnNoContent_WhenSuccessful()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockSender.Setup(s => s.Send(It.Is<DeleteCommand>(c => c.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ErrorOrFactory.From(true));

        // Act
        var result = await _controller.Delete(id);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _mockSender.Verify(s => s.Send(It.Is<DeleteCommand>(c => c.Id == id), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Delete_Should_ReturnNotFound_WhenNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        var errorResult = ErrorOr<bool>.From([
            Error.NotFound("CourtCase.NotFound", "Court case not found")]);

        _mockSender.Setup(s => s.Send(It.IsAny<DeleteCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(errorResult);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(404);
    }


    #endregion
}
