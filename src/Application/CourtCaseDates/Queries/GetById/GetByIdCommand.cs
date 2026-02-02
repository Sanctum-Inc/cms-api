using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.CourtCaseDates.Queries.GetById;

public record GetByIdCommand(Guid Id) : IRequest<ErrorOr<CourtCaseDateResult>>;
