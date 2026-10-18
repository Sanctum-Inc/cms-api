using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using FluentAssertions;
using Infrastructure.Config;
using Infrastructure.Services;
using Microsoft.Extensions.Options;
using Moq;

namespace Api.Tests.Storage;

public class S3FileStorageTests
{
    private const string BucketName = "cms-documents";

    private readonly Mock<IAmazonS3> _clientMock = new();
    private readonly S3FileStorage _storage;

    public S3FileStorageTests()
    {
        _storage = new S3FileStorage(_clientMock.Object, Options.Create(new DocumentStorageOptions
        {
            S3 = new S3StorageOptions { BucketName = BucketName }
        }));
    }

    [Fact]
    public async Task UploadAsync_ShouldPutObjectWithBucketKeyAndContentType()
    {
        // Arrange
        PutObjectRequest? captured = null;
        _clientMock
            .Setup(c => c.PutObjectAsync(It.IsAny<PutObjectRequest>(), It.IsAny<CancellationToken>()))
            .Callback<PutObjectRequest, CancellationToken>((request, _) => captured = request)
            .ReturnsAsync(new PutObjectResponse());

        // Act
        await _storage.UploadAsync("documents/report.pdf", new MemoryStream([1, 2, 3]), "application/pdf");

        // Assert
        captured.Should().NotBeNull();
        captured!.BucketName.Should().Be(BucketName);
        captured.Key.Should().Be("documents/report.pdf");
        captured.ContentType.Should().Be("application/pdf");
        captured.InputStream.Should().NotBeNull();
    }

    [Fact]
    public async Task OpenReadAsync_ShouldReturnStream_FromResponse()
    {
        // Arrange
        var content = new byte[] { 5, 6, 7 };
        var response = new GetObjectResponse { ResponseStream = new MemoryStream(content) };
        _clientMock
            .Setup(c => c.GetObjectAsync(BucketName, "documents/report.pdf", It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        await using var stream = await _storage.OpenReadAsync("documents/report.pdf");
        using var buffer = new MemoryStream();
        await stream.CopyToAsync(buffer);

        // Assert
        buffer.ToArray().Should().Equal(content);
    }

    [Fact]
    public async Task OpenReadAsync_ShouldThrowFileNotFound_WhenClientReturnsNotFound()
    {
        // Arrange
        _clientMock
            .Setup(c => c.GetObjectAsync(BucketName, It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new AmazonS3Exception("Not Found") { StatusCode = HttpStatusCode.NotFound });

        // Act
        var act = () => _storage.OpenReadAsync("documents/missing.pdf");

        // Assert
        await act.Should().ThrowAsync<FileNotFoundException>();
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteObject()
    {
        // Act
        await _storage.DeleteAsync("documents/report.pdf");

        // Assert
        _clientMock.Verify(
            c => c.DeleteObjectAsync(BucketName, "documents/report.pdf", It.IsAny<CancellationToken>()), Times.Once);
    }
}
