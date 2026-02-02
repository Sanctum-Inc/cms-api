using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.CourtCase.Queries.GetCaseNumbers;

public record GetCaseNumbersQuery : IRequest<ErrorOr<IEnumerable<CourtCaseNumberResult>?>>;
