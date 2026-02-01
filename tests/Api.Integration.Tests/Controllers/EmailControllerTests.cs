using System.Net;
using System.Net.Http.Json;
using Contracts.Email.Requests;
using Contracts.Email.Response;
using Domain.Emails;
using FluentAssertions;

namespace Api.Integration.Tests.Controllers;

public class EmailControllerTests : IntegrationTestBase
{
    [Fact]
    public async Task Get_ShouldReturnOk_AndEmails()
    {
        // Act
        var response = await _client.GetAsync("/api/email");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content
            .ReadFromJsonAsync(typeof(IEnumerable<EmailResponse>)) as IEnumerable<EmailResponse>;

        content.Should().NotBeNull();
    }

    [Fact]
    public async Task Create_ShouldReturnCreated_WhenValid()
    {
        // Arrange
        var request = new AddEmailRequest(
            To: new[] { "test@example.com" },
            Cc: null,
            Bcc: null,
            Subject: "Integration Test Email",
            Body: "This is a test email body",
            IsHtml: false,
            AttachmentIds: null
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/email", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequest_WhenInvalid()
    {
        // Arrange
        var request = new AddEmailRequest(
            To: Array.Empty<string>(), // invalid
            Cc: null,
            Bcc: null,
            Subject: "",
            Body: "",
            IsHtml: false,
            AttachmentIds: null
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/email", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenEmailDoesNotExist()
    {
        // Arrange
        var fakeId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/email/{fakeId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
