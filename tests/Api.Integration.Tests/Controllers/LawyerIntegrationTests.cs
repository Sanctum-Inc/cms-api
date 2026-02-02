using System.Net;
using System.Net.Http.Json;
using Contracts.Lawyer.Requests;
using Contracts.Lawyer.Responses;
using FluentAssertions;

namespace Api.Integration.Tests.Controllers;

public class LawyerIntegrationTests : IntegrationTestBase
{
    [Fact]
    public async Task Get_ShouldReturnOk_AndLawyers()
    {
        // Act
        var response = await _client.GetAsync("/api/lawyer");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<List<LawyerResponse>>();
        content.Should().NotBeNull();
        content.Should().NotBeEmpty();
        content![0].Name.Should().NotBeNullOrEmpty();
        content[0].Surname.Should().NotBeNullOrEmpty();
        content[0].Email.Should().NotBeNullOrEmpty();
        content[0].MobileNumber.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetById_WithValidId_ShouldReturnOk_AndLawyer()
    {
        // Arrange - Get a lawyer first to get a valid ID
        var getAllResponse = await _client.GetAsync("/api/lawyer");
        var lawyers = await getAllResponse.Content.ReadFromJsonAsync<List<LawyerResponse>>();
        var validId = lawyers![0].Id;

        // Act
        var response = await _client.GetAsync($"/api/lawyer/{validId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<LawyerResponse>();
        content.Should().NotBeNull();
        content!.Id.Should().Be(validId);
        content.Name.Should().NotBeNullOrEmpty();
        content.Surname.Should().NotBeNullOrEmpty();
        content.Email.Should().NotBeNullOrEmpty();
        content.MobileNumber.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetById_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/lawyer/{invalidId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_WithValidData_ShouldReturnNoContent()
    {
        // Arrange
        var newLawyer = new AddLawyerRequest(
            "test.lawyer@example.com",
            "John",
            "Smith",
            "+27123456789",
            0
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/lawyer", newLawyer);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Create_WithInvalidData_ShouldReturnBadRequest()
    {
        // Arrange - missing required fields
        var invalidLawyer = new AddLawyerRequest(
            "",
            "",
            "",
            "",
            -1
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/lawyer", invalidLawyer);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_WithValidData_ShouldReturnNoContent()
    {
        // Arrange - Get an existing lawyer first
        var getAllResponse = await _client.GetAsync("/api/lawyer");
        var lawyers = await getAllResponse.Content.ReadFromJsonAsync<List<LawyerResponse>>();
        var existingLawyer = lawyers![0];

        var updateRequest = new UpdateLawyerRequest(
            existingLawyer.Email,
            "UpdatedName",
            "UpdatedSurname",
            existingLawyer.MobileNumber,
            1
        );

        // Act
        var response = await _client.PutAsJsonAsync($"/api/lawyer/{existingLawyer.Id}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify the update
        var getResponse = await _client.GetAsync($"/api/lawyer/{existingLawyer.Id}");
        var updatedLawyer = await getResponse.Content.ReadFromJsonAsync<LawyerResponse>();
        updatedLawyer!.Name.Should().Be("UpdatedName");
        updatedLawyer.Surname.Should().Be("UpdatedSurname");
    }

    [Fact]
    public async Task Update_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var invalidId = Guid.NewGuid();
        var updateRequest = new UpdateLawyerRequest(
            "test@example.com",
            "Test",
            "User",
            "+27123456789",
            1
        );

        // Act
        var response = await _client.PutAsJsonAsync($"/api/lawyer/{invalidId}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_WithInvalidData_ShouldReturnBadRequest()
    {
        // Arrange - Get an existing lawyer first
        var getAllResponse = await _client.GetAsync("/api/lawyer");
        var lawyers = await getAllResponse.Content.ReadFromJsonAsync<List<LawyerResponse>>();
        var existingLawyer = lawyers![0];

        var invalidUpdateRequest = new UpdateLawyerRequest(
            "",
            "",
            "",
            "",
            -1
        );

        // Act
        var response = await _client.PutAsJsonAsync($"/api/lawyer/{existingLawyer.Id}", invalidUpdateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Delete_WithValidId_ShouldReturnNoContent()
    {
        // Arrange - Create a lawyer to delete
        var newLawyer = new AddLawyerRequest(
            "delete.test@example.com",
            "Delete",
            "Test",
            "+27987654321",
            3
        );
        await _client.PostAsJsonAsync("/api/lawyer", newLawyer);

        // Get the created lawyer
        var getAllResponse = await _client.GetAsync("/api/lawyer");
        var lawyers = await getAllResponse.Content.ReadFromJsonAsync<List<LawyerResponse>>();
        var lawyerToDelete = lawyers!.First(l => l.Email == "delete.test@example.com");

        // Act
        var response = await _client.DeleteAsync($"/api/lawyer/{lawyerToDelete.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify deletion
        var getResponse = await _client.GetAsync($"/api/lawyer/{lawyerToDelete.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync($"/api/lawyer/{invalidId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task FullCrudWorkflow_ShouldWorkCorrectly()
    {
        // Create
        var createRequest = new AddLawyerRequest(
            "workflow.test@example.com",
            "Workflow",
            "Test",
            "+27111222333",
            4
        );
        var createResponse = await _client.PostAsJsonAsync("/api/lawyer", createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Get all and find created lawyer
        var getAllResponse = await _client.GetAsync("/api/lawyer");
        var lawyers = await getAllResponse.Content.ReadFromJsonAsync<List<LawyerResponse>>();
        var createdLawyer = lawyers!.First(l => l.Email == "workflow.test@example.com");

        // Get by ID
        var getByIdResponse = await _client.GetAsync($"/api/lawyer/{createdLawyer.Id}");
        getByIdResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var retrievedLawyer = await getByIdResponse.Content.ReadFromJsonAsync<LawyerResponse>();
        retrievedLawyer!.Email.Should().Be(createRequest.Email);

        // Update
        var updateRequest = new UpdateLawyerRequest(
            retrievedLawyer.Email,
            "UpdatedWorkflow",
            "UpdatedTest",
            retrievedLawyer.MobileNumber,
            5
        );
        var updateResponse = await _client.PutAsJsonAsync($"/api/lawyer/{createdLawyer.Id}", updateRequest);
        updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify update
        var verifyResponse = await _client.GetAsync($"/api/lawyer/{createdLawyer.Id}");
        var updatedLawyer = await verifyResponse.Content.ReadFromJsonAsync<LawyerResponse>();
        updatedLawyer!.Name.Should().Be("UpdatedWorkflow");

        // Delete
        var deleteResponse = await _client.DeleteAsync($"/api/lawyer/{createdLawyer.Id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify deletion
        var finalGetResponse = await _client.GetAsync($"/api/lawyer/{createdLawyer.Id}");
        finalGetResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
