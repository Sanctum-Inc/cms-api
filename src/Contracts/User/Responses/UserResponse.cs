using Domain.CourtCases;
using Domain.Documents;
using Domain.InvoiceItems;
using Domain.Lawyers;

namespace Contracts.User.Responses;
public record UserResponse(
    Guid Id,
    string Email,
    string Name,
    string Surname,
    string MobileNumber,
    DateTime DateCreated,
    List<CourtCase> CourtCases,
    List<Document> Documents,
    List<InvoiceItem> InvoiceItems,
    List<Lawyer> Lawyers);