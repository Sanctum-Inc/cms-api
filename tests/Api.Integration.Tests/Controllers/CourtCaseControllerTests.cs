using System.Net;
using System.Net.Http.Json;
using Contracts.Common;
using Contracts.CourtCases.Requests;
using Contracts.CourtCases.Responses;
using FluentAssertions;
using Xunit;

namespace Api.Integration.Tests.Controllers;
public class CourtCaseIntegrationTests : IntegrationTestBase
{
    [Fact]
    public async Task Get_ShouldReturnOk_AndCourtCases()
    {
        // Act
        var response = await _client.GetAsync("/api/courtcase");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync(typeof(GetCourtCasesResponse)) as GetCourtCasesResponse;
        content.Should().NotBeNull();
        content.CourtCases?[0].Location.Should().Contain("Johannesburg"); // seeded data check
        content.CourtCases?[0].CaseNumber.Should().Contain("CASE-INT-002"); // seeded data check
        content.CourtCases?[0].Plaintiff.Should().Contain("John"); // seeded data check
        content.CourtCases?[0].Defendant.Should().Contain("Jane"); // seeded data check
        content.CourtCases?[0].Status.Should().Contain("Active"); // seeded data check
        content.CourtCases?[0].Type.Should().Contain("Criminal"); // seeded data check
        content.CourtCases?[0].Outcome.Should().BeNull(); // seeded data check
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
            Status: "Active",
            Type: "Criminal",
            Outcome: null
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
            Status: "Active",
            Type: "Criminal",
            Outcome: null
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
