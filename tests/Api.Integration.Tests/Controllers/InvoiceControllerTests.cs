using System;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Contracts.Invoice.Requests;
using Contracts.Invoice.Responses;
using Contracts.InvoiceItem.Responses;
using FluentAssertions;
using Xunit;

namespace Api.Integration.Tests.Controllers
{
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
                InvoiceNumber: "INV-2025-010",
                InvoiceDate: DateTime.UtcNow,
                ClientName: "Acme Corp",
                Reference: "Ref-010",
                CaseName: "Case X",
                AccountName: "John Doe",
                Bank: "Bank X",
                BranchCode: "123456",
                AccountNumber: "000111222",
                Status: Domain.Invoices.InvoiceStatus.SENT
            );

            var response = await _client.PostAsJsonAsync("/api/invoice", request);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Update_Should_Return_NoContent_And_UpdateInvoice()
        {
            // Arrange: create invoice
            var createRequest = new AddInvoiceRequest(
                "INV-2025-020",
                DateTime.UtcNow,
                "Beta Corp",
                "Ref-020",
                "Case Y",
                "Jane Doe",
                "Bank Y",
                "654321",
                "333444555",
                Status: Domain.Invoices.InvoiceStatus.SENT
            );

            await _client.PostAsJsonAsync("/api/invoice", createRequest);

            // ⚠️ No body is returned, so cannot read createdInvoice from response
            // Instead, fetch all invoices to get the ID
            var invoices = await _client.GetFromJsonAsync<InvoiceResponse[]>("/api/invoice");
            var createdInvoice = invoices!.Single(i => i.InvoiceNumber == createRequest.InvoiceNumber);

            var updateRequest = new AddInvoiceRequest(
                "INV-2025-020-Updated",
                DateTime.UtcNow,
                "Beta Corp Updated",
                "Ref-020-U",
                "Case Y Updated",
                "Jane Doe Updated",
                "Bank Y",
                "654321",
                "333444555",
                Status: Domain.Invoices.InvoiceStatus.SENT
            );

            // Act
            var updateResponse = await _client.PutAsJsonAsync($"/api/invoice/{createdInvoice.Id}", updateRequest);

            // Assert
            updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Confirm updated invoice
            var updatedInvoice = await _client.GetFromJsonAsync<InvoiceResponse>($"/api/invoice/{createdInvoice.Id}");
            updatedInvoice.Should().NotBeNull();
            updatedInvoice!.InvoiceNumber.Should().Be(updateRequest.InvoiceNumber);
            updatedInvoice.ClientName.Should().Be(updateRequest.ClientName);
            updatedInvoice.Reference.Should().Be(updateRequest.Reference);
            updatedInvoice.Plaintiff.Should().Be(updateRequest.CaseName);
            updatedInvoice.Defendant.Should().Be(updateRequest.CaseName);
            updatedInvoice.AccountName.Should().Be(updateRequest.AccountName);
            updatedInvoice.Bank.Should().Be(updateRequest.Bank);
            updatedInvoice.BranchCode.Should().Be(updateRequest.BranchCode);
            updatedInvoice.AccountNumber.Should().Be(updateRequest.AccountNumber);
            updatedInvoice.TotalAmount.Should().Be(0);
        }


        [Fact]
        public async Task Delete_Should_Return_NoContent_And_RemoveInvoice()
        {
            // Arrange: create invoice
            var createRequest = new AddInvoiceRequest(
                "INV-2025-030",
                DateTime.UtcNow,
                "Gamma Corp",
                "Ref-030",
                "Case Z",
                "Alice Doe",
                "Bank Z",
                "112233",
                "777888999",
                Status: Domain.Invoices.InvoiceStatus.SENT
            );

            await _client.PostAsJsonAsync("/api/invoice", createRequest);

            // ⚠️ Fetch invoice ID since POST returns no body
            var invoices = await _client.GetFromJsonAsync<InvoiceResponse[]>("/api/invoice");
            var createdInvoice = invoices!.Single(i => i.InvoiceNumber == createRequest.InvoiceNumber);

            // Act
            var deleteResponse = await _client.DeleteAsync($"/api/invoice/{createdInvoice.Id}");

            // Assert
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Confirm deletion
            var getResponse = await _client.GetAsync($"/api/invoice/{createdInvoice.Id}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

    }
}
