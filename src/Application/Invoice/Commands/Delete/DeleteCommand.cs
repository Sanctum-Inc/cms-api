using ErrorOr;
using MediatR;

namespace Application.Invoice.Commands.Delete;
public record DeleteCommand(
        Guid Id) : IRequest<ErrorOr<bool>>;

