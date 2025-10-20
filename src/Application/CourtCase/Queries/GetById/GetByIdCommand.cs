using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.CourtCase.Queries.GetById;
public record GetByIdCommand(Guid Id) : IRequest<ErrorOr<CourtCaseResult?>>;
