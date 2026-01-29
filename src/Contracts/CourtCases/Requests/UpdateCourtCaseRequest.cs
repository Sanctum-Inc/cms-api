using Domain.CourtCases;
using Domain.Invoices;
using ErrorOr;
using MediatR;

namespace Contracts.CourtCases.Requests;
public record UpdateCourtCaseRequest(
        string CaseNumber,
        string Location,
        string Plaintiff,
        string Defendant,
        CourtCaseStatus Status,
        string? Type,
        string? Outcome) : IRequest<ErrorOr<bool>>;
