using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Application.Common.Interfaces.Services;
using Infrastructure.Config;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class R2FileStorage : IFileStorage
{
    private readonly string _bucketName;
    private readonly IAmazonS3 _client;

    public R2FileStorage(IAmazonS3 client, IOptions<DocumentStorageOptions> options)
    {
        _client = client;
        _bucketName = options.Value.R2.BucketName;
    }

    public async Task UploadAsync(string key, Stream content, string contentType,
        CancellationToken cancellationToken = default)
    {
        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = key,
            InputStream = content,
            ContentType = contentType
        };

        await _client.PutObjectAsync(request, cancellationToken);
    }

    public async Task<Stream> OpenReadAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _client.GetObjectAsync(_bucketName, key, cancellationToken);
            return response.ResponseStream;
        }
        catch (AmazonS3Exception exception) when (exception.StatusCode == HttpStatusCode.NotFound)
        {
            throw new FileNotFoundException($"Object '{key}' was not found.", key);
        }
    }

    public async Task DeleteAsync(string key, CancellationToken cancellationToken = default)
    {
        await _client.DeleteObjectAsync(_bucketName, key, cancellationToken);
    }
}
