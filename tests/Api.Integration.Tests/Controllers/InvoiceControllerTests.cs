using System.Net;
using System.Net.Http.Json;
using Contracts.Invoice.Requests;
using Contracts.Invoice.Responses;
using Domain.Invoices;
using FluentAssertions;

namespace Api.Integration.Tests.Controllers;

public class InvoiceControllerTests : IntegrationTestBase
{
    [Fact]
    public async Task Get_Should_Return_Seeded_Invoices_WithFullProperties()
    {
        // Act
        var response = await _client.GetAsync("/api/invoice");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var invoices = await response.Content.ReadFromJsonAsync<InvoiceResponse[]>();
        invoices.Should().NotBeNull();
        invoices.Should().NotBeEmpty();

        foreach (var invoice in invoices!)
        {
            invoice.Id.Should().NotBeEmpty();
            invoice.InvoiceNumber.Should().NotBeNullOrWhiteSpace();
            invoice.ClientName.Should().NotBeNullOrWhiteSpace();
            invoice.Reference.Should().NotBeNullOrWhiteSpace();
            invoice.Plaintiff.Should().NotBeNullOrWhiteSpace();
            invoice.Defendant.Should().NotBeNullOrWhiteSpace();
            invoice.AccountName.Should().NotBeNullOrWhiteSpace();
            invoice.Bank.Should().NotBeNullOrWhiteSpace();
            invoice.BranchCode.Should().NotBeNullOrWhiteSpace();
            invoice.AccountNumber.Should().NotBeNullOrWhiteSpace();
        }
    }

    [Fact]
    public async Task GetById_Should_Return_Seeded_Invoice_WithFullProperties()
    {
        var seededInvoiceId = "c91d2a2c-1a3e-4a1a-aaa0-1f6b091f7f33";

        var response = await _client.GetAsync($"/api/invoice/{seededInvoiceId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var invoice = await response.Content.ReadFromJsonAsync<InvoiceResponse>();
        invoice.Should().NotBeNull();
        invoice!.Id.Should().Be(Guid.Parse(seededInvoiceId));
    }

    [Fact]
    public async Task Create_Should_Return_Created_Invoice()
    {
        var request = new AddInvoiceRequest(
            "INV-2025-010",
            DateTime.UtcNow,
            "Acme Corp",
            "Ref-010",
            "Case X",
            "John Doe",
            "Bank X",
            "123456",
            "000111222",
            InvoiceStatus.SENT
        );

        var response = await _client.PostAsJsonAsync("/api/invoice", request);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Update_Should_Return_NoContent_And_UpdateInvoice()
    {
        // Arrange
        var request = new UpdateInvoiceRequest(
            new Guid("c91d2a2c-1a3e-4a1a-aaa0-1f6b091f7f33"),
            "INV-2025-010",
            DateTime.UtcNow,
            "Acme Corp",
            "Ref-010",
            "Case X",
            "John Doe",
            "Bank X",
            "123456",
            "000111222",
            InvoiceStatus.SENT
        );

        // Act
        var response = await _client.PutAsJsonAsync(
            $"/api/Invoice/{"c91d2a2c-1a3e-4a1a-aaa0-1f6b091f7f33"}",
            request);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var body = await response.Content.ReadAsStringAsync();
        Assert.True(string.IsNullOrWhiteSpace(body));
    }


    [Fact]
    public async Task Delete_Should_Return_NoContent_And_RemoveInvoice()
    {
        // Act
        var response = await _client.DeleteAsync($"/api/Invoice/{"c91d2a2c-1a3e-4a1a-aaa0-1f6b091f7f33"}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var body = await response.Content.ReadAsStringAsync();
        Assert.True(string.IsNullOrWhiteSpace(body));
    }
}
