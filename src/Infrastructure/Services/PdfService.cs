using System.Security.Cryptography;
using System.Text;
using Application.Common.Interfaces.Services;
using Domain.Firms;
using Domain.Invoices;
using Infrastructure.Config;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Infrastructure.Services;

public class PdfService : IPdfService
{
    public Firm Firm { set; get;  }
    public Invoice Invoice { set; get; }
    public DocumentMetadata GetMetadata()
    {
        return DocumentMetadata.Default;
    }

    public void Compose(IDocumentContainer container)
    {
        var totalAmount = Invoice.Items.Sum(x => x.CostPerHour * x.Hours);

        container.Page(page =>
        {
            page.Margin(50);
            page.Size(PageSizes.A4);

            page.Header().Column(column =>
            {
                column.Spacing(3);

                column.Item().Text(Firm.Name).Bold().FontSize(16);
                column.Item().Text($"Date of admission as an Attorney: {Firm.AttorneyAdmissionDate}").FontSize(10);
                column.Item().Text($"Date of admission as an Advocate: {Firm.AdvocateAdmissionDate}").FontSize(10);

                column.Item().PaddingTop(5).Text(Firm.Address).FontSize(10);
                column.Item().Text($"TEL: {Firm.Telephone}").FontSize(10);
                column.Item().Text($"FAX: {Firm.Fax}").FontSize(10);
                column.Item().Text($"MOBILE: {Firm.Mobile}").FontSize(10);

                column.Item().PaddingTop(10).PaddingBottom(5).LineHorizontal(1);
            });

            page.Content().PaddingTop(20).Column(column =>
            {
                column.Spacing(10);

                // Client info section
                column.Item().Text($"TO: {Invoice.ClientName}").FontSize(11);
                column.Item().Text($"REF: {Invoice.Reference}").FontSize(11);
                column.Item().Text($"RE: {Invoice.ClientName}").FontSize(11);

                column.Item().PaddingTop(10).Table(table =>
                {
                    table.ColumnsDefinition(c =>
                    {
                        c.ConstantColumn(90);
                        c.RelativeColumn(3);
                        c.ConstantColumn(90);
                        c.ConstantColumn(100);
                    });

                    // Header with borders
                    table.Header(header =>
                    {
                        header.Cell().Border(1).Padding(5).Text("DATE").Bold().FontSize(10);
                        header.Cell().Border(1).Padding(5).Text("PARTICULARS").Bold().FontSize(10);
                        header.Cell().Border(1).Padding(5).Text("FEE/AMOUNT").Bold().FontSize(10);
                        header.Cell().Border(1).Padding(5).AlignRight().Text("AMOUNT").Bold().FontSize(10);
                    });

                    // Data rows with borders
                    foreach (var item in Invoice.Items)
                    {
                        table.Cell().Border(1).Padding(5).Text(item.Created.ToString("yyyy/MM/dd")).FontSize(10);

                        table.Cell().Border(1).Padding(5).Column(col => { col.Item().Text(item.Name).FontSize(10); });

                        table.Cell().Border(1).Padding(5).Column(col =>
                        {
                            col.Item().Text($"{item.Hours} hours @ R{item.CostPerHour:N2} p/h").FontSize(10);
                        });

                        var amount = item.Hours * item.CostPerHour;
                        table.Cell().Border(1).Padding(5).AlignRight().Text($"R{amount:N2}").FontSize(10);
                    }

                    // Footer/Total row
                    table.Footer(footer =>
                    {
                        footer.Cell().ColumnSpan(3).Border(1).Padding(5).AlignRight().Text("TOTAL").Bold().FontSize(11);
                        footer.Cell().Border(1).Padding(5).AlignRight().Text($"R{totalAmount:N2}").Bold().FontSize(11);
                    });
                });

                // Banking details section
                column.Item().PaddingTop(20).Text("Banking Details").Bold().FontSize(12);
                column.Item().PaddingTop(5).Text($"Account Name: {Firm.AccountName}").FontSize(10);
                column.Item().Text($"Bank: {Firm.Bank}").FontSize(10);
                column.Item().Text($"Branch Code: {Firm.BranchCode}").FontSize(10);
                column.Item().Text($"Account No: {Firm.AccountNumber}").FontSize(10);

                // Signature section
                column.Item().PaddingTop(30).Text("YOURS FAITHFULLY").FontSize(11);
                column.Item().PaddingTop(5).Text(Firm.Name).Bold().FontSize(11);
            });

            page.Footer().AlignRight().Column(column =>
            {
                column.Item().Text($"EMAIL: {Firm.Email}").FontSize(9);
                column.Item().Text($"INVOICE NO: {Invoice.InvoiceNumber}").FontSize(9);
                column.Item().Text($"DATE: {Invoice.InvoiceDate:dd-MMM-yyyy}").FontSize(9);
            });
        });
    }

    /// <summary>Generate a signed PDF URL</summary>
    public string GenerateSignedPdfUrl(Guid id, string scheme, string host)
    {
        var expires = DateTimeOffset.UtcNow.AddMinutes(1).ToUnixTimeSeconds();
        var sig = Sign($"{id}:{expires}");
        return $"{scheme}://{host}/api/invoice/pdf/view/{id}?exp={expires}&sig={sig}";
    }

    /// <summary>Check if the provided signature is valid</summary>
    public bool IsValidSignature(Guid id, long exp, string sig)
    {
        var expected = Sign($"{id}:{exp}");
        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(sig),
            Encoding.UTF8.GetBytes(expected)
        );
    }

    public string Sign(string data)
    {
        var keyBytes = Encoding.UTF8.GetBytes("_options.SigningKey");
        using var hmac = new HMACSHA256(keyBytes);
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        return WebEncoders.Base64UrlEncode(hash);
    }
}
