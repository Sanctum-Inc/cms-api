using ErrorOr;
using MediatR;

namespace Application.CourtCaseDates.Commands.Delete;

public record DeleteCommand(Guid Id) : IRequest<ErrorOr<bool>>;
