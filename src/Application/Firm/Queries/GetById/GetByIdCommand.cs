using Application.Common.Models;
using MediatR;
using ErrorOr;

namespace Application.Firm.Queries.GetById;

public record GetByIdCommand(Guid Id) : IRequest<ErrorOr<FirmResult>>;
