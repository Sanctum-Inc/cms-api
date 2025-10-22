using System.Net.Http;
using System.Net.Http.Headers;
using Api.Integration.Tests;
using Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Api.Integration.Tests.Controllers;

public abstract class IntegrationTestBase : IAsyncLifetime
{
    protected CustomWebApplicationFactory<Program> _factory;
    protected HttpClient _client { get; private set; }

    protected IntegrationTestBase()
    {
        // Don't create factory here
    }

    public async Task InitializeAsync()
    {
        // 🔹 Create a new factory for each test with unique database
        _factory = new CustomWebApplicationFactory<Program>();
        _client = _factory.CreateClient();

        // Add test authentication header
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Test");

        // 🔹 Seed the database once
        await _factory.SeedDatabaseAsync();
    }

    public async Task DisposeAsync()
    {
        _client?.Dispose();
        _factory?.Dispose();
        await Task.CompletedTask;
    }
}
