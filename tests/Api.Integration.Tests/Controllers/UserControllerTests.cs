// Tests/UserControllerTests.cs

using System.Net;
using System.Net.Http.Json;
using Contracts.User.Requests;
using Contracts.User.Responses;
using FluentAssertions;

namespace Api.Integration.Tests.Controllers;

public class UserControllerTests : IntegrationTestBase
{
    [Fact]
    public async Task GetUser_Should_Return_Seeded_User()
    {
        // Arrange
        var id = "6ec0df63-8960-46ec-9163-2de98e04d5e9";

        // Act
        var response = await _client.GetAsync($"/api/user/{id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var user = await response.Content.ReadFromJsonAsync<UserResponse>();

        // Assert
        user.Should().NotBeNull();
        user!.Email.Should().Be("testuser@example.com");
        user.Name.Should().Be("testUser");
        user.Surname.Should().Be("testSurname");
        user.MobileNumber.Should().Be("+27812198232");
    }

    [Fact]
    public async Task Register_Should_Create_New_User()
    {
        // Arrange
        var request = new RegisterRequest(
            "newuser@example.com",
            "Tshego",
            "Motlatle",
            "+27821234567",
            "P@ssword123",
            new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

        // Act
        var response = await _client.PostAsJsonAsync("/api/user/register", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Login_Should_Return_Token_If_Valid()
    {
        var request = new LoginRequest("testuser@example.com", "P@ssword123");

        var response = await _client.PostAsJsonAsync("/api/user/login", request);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();

        loginResponse.Should().NotBeNull();
        loginResponse!.Token.Should().NotBeNullOrWhiteSpace();
    }
}
