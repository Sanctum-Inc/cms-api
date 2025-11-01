using System.Net;
using System.Net.Http.Json;
using Contracts.CourtCaseDates.Requests;
using Contracts.CourtCaseDates.Responses;
using FluentAssertions;

namespace Api.Integration.Tests.Controllers;

public class CourtCaseDatesControllerTests : IntegrationTestBase
{
    [Fact]
    public async Task Get_ShouldReturnOk_AndCourtCaseDates()
    {
        // Act
        var response = await _client.GetAsync("/api/courtcasedate");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync(typeof(IEnumerable<CourtCaseDatesResponse>)) as IEnumerable<CourtCaseDatesResponse>;
        content.Should().NotBeNull();
    }

    [Fact]
    public async Task Create_ShouldReturnCreated_WhenValid()
    {
        // Arrange
        var request = new AddCourtCaseDateRequest(
            Date: "2025-10-31",
            Title: "Hearing",
            CaseId: new Guid("9ae37995-fb0f-4f86-8f9f-30068950df4c")
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/courtcasedate", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequest_WhenInvalid()
    {
        // Arrange
        var request = new AddCourtCaseDateRequest(
            Date: "",
            Title: "",
            CaseId: Guid.Empty
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/courtcasedate", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenDateDoesNotExist()
    {
        // Arrange
        var fakeId = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync($"/api/courtcasedate/{fakeId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}

