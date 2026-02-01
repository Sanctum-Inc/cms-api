using System.Net;
using System.Net.Http.Json;
using Contracts.CourtCases.Requests;
using Contracts.CourtCases.Responses;
using Domain.CourtCases;
using FluentAssertions;

namespace Api.Integration.Tests.Controllers;
public class CourtCaseControllerTests : IntegrationTestBase
{
    [Fact]
    public async Task Get_ShouldReturnOk_AndCourtCases()
    {
        // Act
        var response = await _client.GetAsync("/api/courtcase");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync(typeof(IEnumerable<CourtCasesResponse>)) as IEnumerable<CourtCasesResponse>;
        content.Should().NotBeNull();
    }

    [Fact]
    public async Task Create_ShouldReturnCreated_WhenValid()
    {
        // Arrange
        var request = new AddCourtCaseRequest(
            CaseNumber: "CASE-INT-002",
            Location: "Johannesburg",
            Plaintiff: "John",
            Defendant: "Jane",
            Status: CourtCaseStatus.Draft,
            Type: CourtCaseTypes.Commercial,
            Outcome: CourtCaseOutcomes.Liable
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/courtcase", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequest_WhenInvalid()
    {
        // Arrange
        var request = new AddCourtCaseRequest(
            CaseNumber: "",
            Location: "",
            Plaintiff: "",
            Defendant: "",
            Status: CourtCaseStatus.Draft,
            Type: CourtCaseTypes.Civil,
            Outcome: CourtCaseOutcomes.Guilty
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/courtcase", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenCaseDoesNotExist()
    {
        // Arrange
        var fakeId = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync($"/api/courtcase/{fakeId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
