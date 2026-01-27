using Domain.Invoices;

namespace Contracts.CourtCases.Requests;
public record AddCourtCaseRequest(
        string CaseNumber,
        string Location,
        string Plaintiff,
        string Defendant,
        InvoiceStatus Status,
        string? Type,
        string? Outcome);
