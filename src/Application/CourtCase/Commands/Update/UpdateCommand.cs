using ErrorOr;
using MediatR;

namespace Application.CourtCase.Commands.Update;
public record UpdateCommand(
        Guid Id,
        string CaseNumber,
        string Location,
        string Plaintiff,
        string Defendant,
        string Status,
        string? Type,
        string? Outcome) : IRequest<ErrorOr<bool>>;
