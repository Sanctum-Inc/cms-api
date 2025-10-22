using Api.Controllers;
using Application.Common.Models;
using Application.CourtCase.Commands.Add;
using Application.CourtCase.Commands.Delete;
using Application.CourtCase.Commands.Update;
using Application.CourtCase.Queries.Get;
using Contracts.CourtCases.Requests;
using Contracts.CourtCases.Responses;
using ErrorOr;
using FluentAssertions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Api.Tests.Controllers;

public class CourtCaseControllerTests
{
    private readonly Mock<ISender> _mockSender;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CourtCaseController _controller;

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
        new CourtCaseResult
        {
            Id = Guid.NewGuid(),
            CaseNumber = "CASE-2024-001",
            Location = "Johannesburg High Court",
            Plaintiff = "John Doe",
            Defendant = "Jane Smith",
            Status = "Pending",
            Type = "Civil",
            Outcome = null
        }
    };

        var expectedResponse = expectedCases.Select(c => new CourtCasesResponse()
        {
            CaseNumber = c.CaseNumber,
            Location = c.Location,
            Plaintiff = c.Plaintiff,
            Defendant = c.Defendant,
            Status = c.Status,
            Type = c.Type,
            Outcome = c.Outcome,
            Id = c.Id,
            UserId = c.UserId,
            Lawyers = c.Lawyers,
            User = c.User
        });

        _mockSender
            .Setup(s => s.Send(It.IsAny<GetCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedCases);

        _mockMapper
            .Setup(m => m.Map<IEnumerable<CourtCasesResponse>>(It.IsAny<IEnumerable<CourtCaseResult>>()))
            .Returns((IEnumerable<CourtCaseResult> source) => expectedResponse);

        // Act
        var result = await _controller.Get();

        // Assert
        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(expectedResponse);

        _mockSender.Verify(s => s.Send(It.IsAny<GetCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockMapper.Verify(m => m.Map<IEnumerable<CourtCasesResponse>>(It.IsAny<IEnumerable<CourtCaseResult>>()), Times.Once);
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
            "Pending",
            "Civil",
            null
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
        var request = new AddCourtCaseRequest(
            "",
            "Johannesburg High Court",
            "John Doe",
            "Jane Smith",
            "Pending",
            "Civil",
            null
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
            "Ongoing",
            "Civil",
            null
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
        var request = new UpdateCourtCaseRequest("CASE-404", "Pretoria", "A", "B", "Ongoing", "Civil", null);
        var mappedCommand = new UpdateCommand(Guid.NewGuid(), request.CaseNumber, request.Location, request.Plaintiff, request.Defendant, request.Status, request.Type, request.Outcome);
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
        var error = Error.NotFound("CourtCase.NotFound", "Court case not found");
        _mockSender.Setup(s => s.Send(It.IsAny<DeleteCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(error);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(404);
    }

    #endregion
}
