using Domain.CourtCaseDates;
using Domain.Invoices;
using ErrorOr;
using MediatR;

namespace Application.CourtCase.Commands.Add;
public record AddCommand(
        string CaseNumber,
        string Location,
        string Plaintiff,
        string Defendant,
        CourtCaseStatus Status,
        string? Type,
        string? Outcome) : IRequest<ErrorOr<Guid>>;
