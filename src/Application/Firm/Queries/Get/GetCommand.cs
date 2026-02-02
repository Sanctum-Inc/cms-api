using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.Firm.Queries.Get;

public record GetCommand : IRequest<ErrorOr<FirmResult>>;
