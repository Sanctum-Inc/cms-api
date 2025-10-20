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
        container.Page(page =>
        {
            page.Margin(40);
            page.Size(PageSizes.A4);

            page.Header().Column(column =>
            {
                column.Item().Text(_firm.Name).Bold().FontSize(18);
                column.Item().Text($"Date of admission as Attorney: {_firm.AttorneyAdmissionDate}");
                column.Item().Text($"Date of admission as Advocate: {_firm.AdvocateAdmissionDate}");
                column.Item().Text(_firm.Address);
                column.Item().Text($"TEL: {_firm.Telephone} FAX: {_firm.Fax} MOBILE: {_firm.Mobile}");
                column.Item().Text($"EMAIL: {_firm.Email}");
                column.Item().Text($"INVOICE NO: {_invoice.InvoiceNumber}");
                column.Item().Text($"DATE: {_invoice.InvoiceDate:dd-MMM-yyyy}");
            });

            page.Content().Column(column =>
            {
                column.Spacing(20);

                column.Item().Text($"TO: {_invoice.ClientName}").Bold();
                column.Item().Text($"REF: {_invoice.Reference}");
                column.Item().Text($"RE: {_invoice.CaseName}");

                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(c =>
                    {
                        c.ConstantColumn(100);
                        c.RelativeColumn(2);
                        c.ConstantColumn(80);
                        c.ConstantColumn(100);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Text("DATE").Bold();
                        header.Cell().Text("PARTICULARS").Bold();
                        header.Cell().Text("HOURS/RATE").Bold();
                        header.Cell().Text("AMOUNT").Bold();
                    });

                    foreach (var item in _invoice.Items)
                    {
                        table.Cell().Text(item.Created.ToString("yyyy/MM/dd"));
                        table.Cell().Text(item.Name);

                        if (item.IsDayFee)
                        {
                            table.Cell().Text("Day Fee");
                            table.Cell().Text((item.DayFeeAmount ?? 0m).ToString("C"));
                        }
                        else
                        {
                            table.Cell().Text($"{item.Hours} hrs @ {item.CostPerHour:C}");
                            var amount = item.Hours * (decimal)(item.CostPerHour ?? 0m);
                            table.Cell().Text(amount.ToString("C"));
                        }
                    }

                    table.Footer(footer =>
                    {
                        footer.Cell().ColumnSpan(3).AlignRight().Text("TOTAL").Bold();
                        footer.Cell().Text(_invoice.TotalAmount.ToString("C")).Bold();
                    });
                });

                column.Item().Text("Banking Details").Bold().FontSize(14);
                column.Item().Text($"Account Name: {_firm.AccountName}");
                column.Item().Text($"Bank: {_firm.Bank}");
                column.Item().Text($"Branch Code: {_firm.BranchCode}");
                column.Item().Text($"Account No: {_firm.AccountNumber}");

                column.Item().Text("YOURS FAITHFULLY");
                column.Item().Text(_firm.Name).Bold();
            });
        });
    }
}
