using Application.Common.Interfaces.Services;
using Infrastructure.Config;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class LocalFileStorage : IFileStorage
{
    private readonly string _rootPath;

    public LocalFileStorage(IOptions<DocumentStorageOptions> options)
    {
        _rootPath = options.Value.RootPath;
    }

    public async Task UploadAsync(string key, Stream content, string contentType,
        CancellationToken cancellationToken = default)
    {
        var path = ResolvePath(key);
        var directory = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
        await content.CopyToAsync(stream, cancellationToken);
    }

    public Task<Stream> OpenReadAsync(string key, CancellationToken cancellationToken = default)
    {
        var path = ResolvePath(key);
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"File '{key}' was not found.", key);
        }

        Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        return Task.FromResult(stream);
    }

    public Task DeleteAsync(string key, CancellationToken cancellationToken = default)
    {
        var path = ResolvePath(key);
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        return Task.CompletedTask;
    }

    private string ResolvePath(string key)
    {
        var relativePath = key.Replace('/', Path.DirectorySeparatorChar);
        return Path.Combine(_rootPath, relativePath);
    }
}
