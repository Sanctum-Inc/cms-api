using FluentAssertions;
using Infrastructure.Config;
using Infrastructure.Services;
using Microsoft.Extensions.Options;

namespace Api.Tests.Storage;

public class LocalFileStorageTests : IDisposable
{
    private readonly string _rootPath;
    private readonly LocalFileStorage _storage;

    public LocalFileStorageTests()
    {
        _rootPath = Path.Combine(Path.GetTempPath(), "cms-api-tests", Guid.NewGuid().ToString());
        _storage = new LocalFileStorage(Options.Create(new DocumentStorageOptions { RootPath = _rootPath }));
    }

    [Fact]
    public async Task UploadAsync_ShouldWriteFile_AndOpenReadAsyncShouldReturnContent()
    {
        // Arrange
        var content = new byte[] { 1, 2, 3, 4 };

        // Act
        await _storage.UploadAsync("documents/report.pdf", new MemoryStream(content), "application/pdf");

        // Assert
        File.Exists(Path.Combine(_rootPath, "documents", "report.pdf")).Should().BeTrue();

        await using var stream = await _storage.OpenReadAsync("documents/report.pdf");
        using var buffer = new MemoryStream();
        await stream.CopyToAsync(buffer);
        buffer.ToArray().Should().Equal(content);
    }

    [Fact]
    public async Task OpenReadAsync_ShouldThrowFileNotFound_WhenKeyDoesNotExist()
    {
        // Act
        var act = () => _storage.OpenReadAsync("documents/missing.pdf");

        // Assert
        await act.Should().ThrowAsync<FileNotFoundException>();
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveFile_AndNotThrow_WhenKeyDoesNotExist()
    {
        // Arrange
        await _storage.UploadAsync("documents/report.pdf", new MemoryStream([1]), "application/pdf");

        // Act
        await _storage.DeleteAsync("documents/report.pdf");
        await _storage.DeleteAsync("documents/missing.pdf");

        // Assert
        File.Exists(Path.Combine(_rootPath, "documents", "report.pdf")).Should().BeFalse();
    }

    public void Dispose()
    {
        if (Directory.Exists(_rootPath))
        {
            Directory.Delete(_rootPath, true);
        }
    }
}
