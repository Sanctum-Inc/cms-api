using Domain.Documents;
using Domain.InvoiceItems;
using Domain.Lawyers;

namespace Application.Users.Queries;
public class UserResult
{
    public required Guid Id { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string MobileNumber { get; set; }
    public required DateTime Created { get; set; }

    // Relations
    public List<Domain.CourtCases.CourtCase> CourtCases { get; set; } = [];
    public List<Domain.Documents.Document> Documents { get; set; } = [];
    public List<InvoiceItem> InvoiceItems { get; set; } = [];
    public List<Domain.Lawyers.Lawyer> Lawyers { get; set; } = [];
}
