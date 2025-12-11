namespace Infrastructure.Services;
using Domain.Firms;
using Domain.Invoices;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

public class PdfService : IDocument
{
    private readonly Invoice _invoice;
    private readonly Firm _firm;

    public PdfService(Invoice invoice, Firm firm)
    {
        _invoice = invoice;
        _firm = firm;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        var totalAmount = _invoice.Items.Sum(x => x.CostPerHour * x.Hours);

        container.Page(page =>
        {
            page.Margin(50);
            page.Size(PageSizes.A4);

            page.Header().Column(column =>
            {
                column.Spacing(3);

                column.Item().Text(_firm.Name).Bold().FontSize(16);
                column.Item().Text($"Date of admission as an Attorney: {_firm.AttorneyAdmissionDate}").FontSize(10);
                column.Item().Text($"Date of admission as an Advocate: {_firm.AdvocateAdmissionDate}").FontSize(10);

                column.Item().PaddingTop(5).Text(_firm.Address).FontSize(10);
                column.Item().Text($"TEL: {_firm.Telephone}").FontSize(10);
                column.Item().Text($"FAX: {_firm.Fax}").FontSize(10);
                column.Item().Text($"MOBILE: {_firm.Mobile}").FontSize(10);

                column.Item().PaddingTop(10).PaddingBottom(5).LineHorizontal(1);
            });

            page.Content().PaddingTop(20).Column(column =>
            {
                column.Spacing(10);

                // Client info section
                column.Item().Text($"TO: {_invoice.ClientName}").FontSize(11);
                column.Item().Text($"REF: {_invoice.Reference}").FontSize(11);
                column.Item().Text($"RE: {_invoice.ClientName}").FontSize(11);

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
                    foreach (var item in _invoice.Items)
                    {
                        table.Cell().Border(1).Padding(5).Text(item.Created.ToString("yyyy/MM/dd")).FontSize(10);

                        table.Cell().Border(1).Padding(5).Column(col =>
                        {
                            col.Item().Text(item.Name).FontSize(10);
                        });

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
                column.Item().PaddingTop(5).Text($"Account Name: {_firm.AccountName}").FontSize(10);
                column.Item().Text($"Bank: {_firm.Bank}").FontSize(10);
                column.Item().Text($"Branch Code: {_firm.BranchCode}").FontSize(10);
                column.Item().Text($"Account No: {_firm.AccountNumber}").FontSize(10);

                // Signature section
                column.Item().PaddingTop(30).Text("YOURS FAITHFULLY").FontSize(11);
                column.Item().PaddingTop(5).Text(_firm.Name).Bold().FontSize(11);
            });

            page.Footer().AlignRight().Column(column =>
            {
                column.Item().Text($"EMAIL: {_firm.Email}").FontSize(9);
                column.Item().Text($"INVOICE NO: {_invoice.InvoiceNumber}").FontSize(9);
                column.Item().Text($"DATE: {_invoice.InvoiceDate:dd-MMM-yyyy}").FontSize(9);
            });
        });
    }
}
