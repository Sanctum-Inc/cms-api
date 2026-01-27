using ErrorOr;
using MediatR;

namespace Application.Firm.Commands.Delete;
public record DeleteCommand(Guid Id) : IRequest<ErrorOr<bool>>;
