using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.CourtCase.Queries.Get;

public record GetQuery() : IRequest<ErrorOr<IEnumerable<CourtCaseResult>>>;
