using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Api.Integration.Tests.Controllers;
using Contracts.InvoiceItem.Requests;
using Contracts.InvoiceItem.Responses;
using FluentAssertions;
using Xunit;

namespace Api.Integration.Tests.Controllers;

public class InvoiceItemIntegrationTests : IntegrationTestBase
{
    // 🔹 Use IDs that match the seeded data
    private static readonly Guid ExistingInvoiceItemId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid ExistingInvoiceId = Guid.Parse("c91d2a2c-1a3e-4a1a-aaa0-1f6b091f7f33");
    private static readonly Guid ExistingCourtCaseId = Guid.Parse("9ae37995-fb0f-4f86-8f9f-30068950df4c");

    [Fact]
    public async Task Get_ShouldReturnOk_AndSeededInvoiceItems()
    {
        // Act
        var response = await _client.GetAsync("/api/invoiceitem");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<IEnumerable<InvoiceItemResponse>>();
        content.Should().NotBeNullOrEmpty();

        var items = content!.ToList();
        items.Should().HaveCountGreaterThanOrEqualTo(1);

        var item = items.First(i => i.Id == ExistingInvoiceItemId);
        item.Name.Should().Be("Consultation Fee");
        item.Hours.Should().Be(2);
        item.CostPerHour.Should().Be(1500m);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_AndCorrectInvoiceItem()
    {
        // Act
        var response = await _client.GetAsync($"/api/invoiceitem/{ExistingInvoiceItemId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var item = await response.Content.ReadFromJsonAsync<InvoiceItemResponse>();
        item.Should().NotBeNull();
        item!.Id.Should().Be(ExistingInvoiceItemId);
        item.Name.Should().Be("Consultation Fee");
    }

    [Fact]
    public async Task Create_ShouldReturnCreated_AndPersistNewInvoiceItem()
    {
        // Arrange
        var request = new AddInvoiceItemRequest(
            InvoiceId: ExistingInvoiceId.ToString(),
            Name: "Drafting of Affidavit",
            Hours: 3,
            CostPerHour: 950,
            CaseId: ExistingCourtCaseId,
            ClientName: "",
            Refference: "",
            Date: "2016/01/01"
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/invoiceitem", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        // Confirm via GET
        var listResponse = await _client.GetAsync("/api/invoiceitem");
        var list = await listResponse.Content.ReadFromJsonAsync<IEnumerable<InvoiceItemResponse>>();
        list.Should().Contain(x => x.Name == "Drafting of Affidavit");
    }

    [Fact]
    public async Task Update_ShouldReturnNoContent_AndModifyExistingInvoiceItem()
    {
        // Arrange
        var request = new UpdateInvoiceItemRequest(
            InvoiceId: ExistingInvoiceId,
            Name: "Consultation Fee (Updated)",
            Hours: 4,
            CostPerHour: 1550,
            CaseId: ExistingCourtCaseId
        );

        // Act
        var response = await _client.PutAsJsonAsync($"/api/invoiceitem/{ExistingInvoiceItemId}", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Confirm update
        var getResponse = await _client.GetAsync($"/api/invoiceitem/{ExistingInvoiceItemId}");
        var updated = await getResponse.Content.ReadFromJsonAsync<InvoiceItemResponse>();
        updated.Should().NotBeNull();
        updated!.Name.Should().Be("Consultation Fee (Updated)");
        updated.Hours.Should().Be(4);
        updated.CostPerHour.Should().Be(1550);
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent_AndRemoveInvoiceItem()
    {
        // Arrange - Use the second seeded invoice item
        var toDeleteId = Guid.Parse("44444444-4444-4444-4444-444444444444");

        // Act
        var response = await _client.DeleteAsync($"/api/invoiceitem/{toDeleteId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Confirm deletion
        var getResponse = await _client.GetAsync($"/api/invoiceitem/{toDeleteId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
