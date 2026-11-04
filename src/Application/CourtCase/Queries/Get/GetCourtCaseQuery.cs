using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.CourtCase.Queries.Get;

public record GetCourtCaseQuery : IRequest<ErrorOr<IEnumerable<CourtCaseResult>>>;
