using System.Net.Http;
using Api.Integration.Tests;
using Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Api.Integration.Tests.Controllers;

public abstract class IntegrationTestBase : IAsyncLifetime
{
    protected readonly CustomWebApplicationFactory<Program> _factory;
    protected HttpClient _client { get; private set; }

    protected IntegrationTestBase()
    {
        _factory = new CustomWebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    public async Task InitializeAsync()
    {
        // Reset DB before each test
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
        _factory.ResetDatabase(db);

        await Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        // Optional: clean up resources after test
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
        _factory.Dispose(db);

        _client.Dispose();
        await Task.CompletedTask;
    }
}
