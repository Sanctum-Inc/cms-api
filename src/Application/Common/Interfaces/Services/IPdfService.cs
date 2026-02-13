using QuestPDF.Infrastructure;

namespace Application.Common.Interfaces.Services;

public interface IPdfService : IDocument
{
    Domain.Firms.Firm Firm { set; get;  }
    Domain.Invoices.Invoice Invoice { set; get; }

    bool IsValidSignature(Guid id, long exp, string sig);

    string GenerateSignedPdfUrl(Guid id, string scheme, string host);

    string Sign(string data);
}
