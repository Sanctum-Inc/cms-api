using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Contracts.Documents.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Api.Integration.Tests.Controllers;

public class DocumentControllerTests : IntegrationTestBase
{
    [Fact]
    public async Task GetStructure_Should_Return_Seeded_Documents()
    {
        // Act
        var response = await _client.GetAsync("/api/document");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var documents = await response.Content.ReadFromJsonAsync<DocumentResponse[]>();

        // Assert
        documents.Should().NotBeNull();
        documents.Should().NotBeEmpty();
        documents![0].Name.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task GetById_Should_Return_Seeded_Document()
    {
        // ⚠️ Use a known seeded document ID from your test DB seed
        var seededDocumentId = "8f1b1dbf-0c63-4e0e-a16c-4cc78e66ad98";

        var response = await _client.GetAsync($"/api/document/{seededDocumentId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var document = await response.Content.ReadFromJsonAsync<DocumentResponse>();
        document.Should().NotBeNull();
        document!.Id.Should().Be(Guid.Parse(seededDocumentId));
    }

    [Fact]
    public async Task Upload_Should_Create_New_Document()
    {
        // Arrange
        using var fileStream = new MemoryStream(new byte[] { 1, 2, 3, 4, 5 });
        using var content = new MultipartFormDataContent();

        var fileContent = new StreamContent(fileStream);
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");

        content.Add(fileContent, "file", "testdoc.pdf");
        content.Add(new StringContent("Test Document"), "name");
        content.Add(new StringContent("9ae37995-fb0f-4f86-8f9f-30068950df4c"), "caseId");

        // Act
        var response = await _client.PostAsync("/api/document/upload", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task UpdateName_Should_Update_Document_Name()
    {
        // ⚠️ Use a seeded document ID
        var seededDocumentId = "8f1b1dbf-0c63-4e0e-a16c-4cc78e66ad98";
        var updatePayload = new { FileName = "updated_name.pdf" };

        var response = await _client.PutAsJsonAsync($"/api/document/{seededDocumentId}", updatePayload);
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Download_Should_Return_File_Stream()
    {
        // ⚠️ Use a seeded document ID that has a file
        var seededDocumentId = "8f1b1dbf-0c63-4e0e-a16c-4cc78e66ad98";

        var response = await _client.GetAsync($"/api/document/{seededDocumentId}/download");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType!.MediaType.Should().Be("text/plain");

        var bytes = await response.Content.ReadAsByteArrayAsync();
        bytes.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Delete_Should_Remove_Document()
    {
        var id = "8f1b1dbf-0c63-4e0e-a16c-4cc78e66ad98";

        // Act
        var deleteResponse = await _client.DeleteAsync($"/api/document/{id}");

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
