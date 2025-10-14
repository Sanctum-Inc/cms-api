using ErrorOr;
using MediatR;

namespace Application.CourtCase.Queries.Get;

public record GetCommand() : IRequest<ErrorOr<GetCourtCaseResult>>;