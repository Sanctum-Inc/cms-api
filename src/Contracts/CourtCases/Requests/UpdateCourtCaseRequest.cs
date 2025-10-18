using ErrorOr;
using MediatR;

namespace Contracts.CourtCases.Requests;
public record UpdateCourtCaseRequest(
        string CaseNumber,
        string Location,
        string Plaintiff,
        string Defendant,
        string Status,
        string? Type,
        string? Outcome) : IRequest<ErrorOr<bool>>;
