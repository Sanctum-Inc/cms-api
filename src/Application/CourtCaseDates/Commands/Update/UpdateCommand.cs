using ErrorOr;
using MediatR;

namespace Application.CourtCaseDates.Commands.Update;
public record UpdateCommand(
    Guid Id,
    string Date,
    string Title,
    Guid CaseId
) : IRequest<ErrorOr<bool>>;
