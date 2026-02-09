using Api.Controllers;
using Application.Common.Models;
using Application.CourtCaseDates.Commands.Add;
using Application.CourtCaseDates.Commands.Delete;
using Application.CourtCaseDates.Commands.SetToCancelled;
using Application.CourtCaseDates.Commands.SetToComplete;
using Application.CourtCaseDates.Commands.Update;
using Application.CourtCaseDates.Queries.Get;
using Application.CourtCaseDates.Queries.GetById;
using Contracts.CourtCaseDates.Requests;
using Contracts.CourtCaseDates.Responses;
using Domain.CourtCaseDates;
using ErrorOr;
using FluentAssertions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Api.Tests.Controllers;

public class CourtCaseDateControllerTests
{
    private readonly CourtCaseDateController _controller;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<ISender> _sender;

    public CourtCaseDateControllerTests()
    {
        _mapper = new Mock<IMapper>(MockBehavior.Strict);
        _sender = new Mock<ISender>(MockBehavior.Strict);
        _controller = new CourtCaseDateController(_mapper.Object, _sender.Object);
    }

    #region GET ALL

    [Fact]
    public async Task GetAll_Should_ReturnOk_WithDashboardResult()
    {
        // Arrange
        var deadlineItem = CreateItem();
        var items = new List<CourtCaseDateItem> { CreateItem() };

        var resultModel = new CourtCaseDateResult(
            1, 80, 3, 12.5f, deadlineItem, items);

        var responseModel = CreateResponse(deadlineItem, items);

        _sender.Setup(s => s.Send(It.IsAny<GetCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(resultModel);

        _mapper.Setup(m => m.Map<CourtCaseDateResponse>(resultModel))
            .Returns(responseModel);

        // Act
        var result = await _controller.GetAll();

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(responseModel);
    }

    #endregion

    #region GET BY ID

    [Fact]
    public async Task GetById_Should_ReturnOk_WhenFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var item = CreateItem();
        var resultModel = new CourtCaseDateResult(0, 100, 0, 0, item, new[] { item });
        var responseModel = CreateResponse(item, new[] { item });

        _sender.Setup(s => s.Send(It.Is<GetByIdCommand>(c => c.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(resultModel);

        _mapper.Setup(m => m.Map<CourtCaseDateResponse>(resultModel))
            .Returns(responseModel);

        // Act
        var result = await _controller.GetById(id);

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(responseModel);
    }

    [Fact]
    public async Task GetById_Should_ReturnNotFound_WhenMissing()
    {
        // Arrange
        var id = Guid.NewGuid();
        var error = Error.NotFound("CourtCaseDate.NotFound", "Not found");

        _sender.Setup(s => s.Send(It.IsAny<GetByIdCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(error);

        // Act
        var result = await _controller.GetById(id);

        // Assert
        result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(404);
    }

    #endregion

    #region CREATE

    [Fact]
    public async Task Create_Should_ReturnCreated_WhenSuccessful()
    {
        // Arrange
        var request = new AddCourtCaseDateRequest(
            "2026-01-01",
            "Hearing",
            "",
            Guid.NewGuid(),
            CourtCaseDateTypes.Hearing);

        var command = new AddCommand(
            request.Date,
            request.Title,
            "subtitle",
            request.CaseId,
            request.Type);

        _mapper.Setup(m => m.Map<AddCommand>(request)).Returns(command);
        _sender.Setup(s => s.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Guid());

        // Act
        var result = await _controller.Create(request);

        // Assert
        result.Should().BeOfType<CreatedResult>();
    }

    [Fact]
    public async Task Create_Should_ReturnBadRequest_OnValidationError()
    {
        // Arrange
        var request = new AddCourtCaseDateRequest(
            "",
            "",
            "",
            Guid.NewGuid(),
            CourtCaseDateTypes.Hearing);

        var command = new AddCommand("", "", "subtitle", Guid.Empty, CourtCaseDateTypes.Hearing);
        var error = Error.Validation("Invalid", "Invalid input");

        _mapper.Setup(m => m.Map<AddCommand>(request)).Returns(command);
        _sender.Setup(s => s.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(error);

        // Act
        var result = await _controller.Create(request);

        // Assert
        result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(400);
    }

    #endregion

    #region UPDATE

    [Fact]
    public async Task Update_Should_ReturnNoContent_WhenSuccessful()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = new UpdateCourtCaseDateRequest("2026-01-01", "Title", "Test",CourtCaseDateTypes.Arraignment, false, false, Guid.NewGuid());
        var command = new UpdateCommand(id, request.Date, request.Title, "Test", false, false, CourtCaseDateTypes.Mediation, request.CaseId);

        _mapper.Setup(m => m.Map<UpdateCommand>(request)).Returns(command);
        _sender.Setup(s => s.Send(It.Is<UpdateCommand>(c => c.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ErrorOrFactory.From(true));

        // Act
        var result = await _controller.Update(id, request);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Update_Should_ReturnNotFound_WhenMissing()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = new UpdateCourtCaseDateRequest("2026-01-01", "Title", "Test",CourtCaseDateTypes.Arraignment, false, false, Guid.NewGuid());
        var command = new UpdateCommand(id, request.Date, request.Title, "Test", false, false, CourtCaseDateTypes.Mediation, request.CaseId);

        _mapper.Setup(m => m.Map<UpdateCommand>(request))
            .Returns(command);

        _sender.Setup(s => s.Send(It.IsAny<UpdateCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Error.NotFound());

        // Act
        var result = await _controller.Update(id, request);

        // Assert
        result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(404);
    }

    #endregion

    #region PATCH

    [Fact]
    public async Task SetToCancelled_Should_ReturnNoContent()
    {
        var id = Guid.NewGuid();

        _sender.Setup(s => s.Send(It.Is<SetToCancelledCommand>(c => c.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ErrorOrFactory.From(true));

        var result = await _controller.SetToCancelled(id);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task SetToComplete_Should_ReturnNoContent()
    {
        var id = Guid.NewGuid();

        _sender.Setup(s => s.Send(It.Is<SetToCompleteCommand>(c => c.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ErrorOrFactory.From(true));

        var result = await _controller.SetToComplete(id);

        result.Should().BeOfType<NoContentResult>();
    }

    #endregion

    #region DELETE

    [Fact]
    public async Task Delete_Should_ReturnNoContent_WhenSuccessful()
    {
        var id = Guid.NewGuid();

        _sender.Setup(s => s.Send(It.Is<DeleteCommand>(c => c.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ErrorOrFactory.From(true));

        var result = await _controller.Delete(id);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Delete_Should_ReturnNotFound_WhenMissing()
    {
        var error = Error.NotFound("NotFound", "Missing");

        _sender.Setup(s => s.Send(It.IsAny<DeleteCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(error);

        var result = await _controller.Delete(Guid.NewGuid());

        result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(404);
    }

    #endregion

    #region Helpers

    private static CourtCaseDateItem CreateItem() =>
        new(
            Guid.NewGuid(),
            "2026-02-10",
            "Hearing",
            "CC-123",
            Guid.NewGuid(),
            CourtCaseDateTypes.Hearing,
            "Subtitle",
            "Description",
            "Upcoming"
        );

    private static CourtCaseDateResponse CreateResponse(
        CourtCaseDateItem deadline,
        IEnumerable<CourtCaseDateItem> items) =>
        new(
            1,
            80,
            3,
            12.5f,
            new CourtCaseDateItemResponse(
                deadline.Id,
                deadline.Date,
                deadline.Title,
                deadline.CaseNumber,
                deadline.CaseId,
                deadline.CourtCaseDateType,
                deadline.Subtitle,
                deadline.Description,
                deadline.Status),
            items.Select(i => new CourtCaseDateItemResponse(
                i.Id,
                i.Date,
                i.Title,
                i.CaseNumber,
                i.CaseId,
                i.CourtCaseDateType,
                i.Subtitle,
                i.Description,
                i.Status))
        );

    #endregion
}
