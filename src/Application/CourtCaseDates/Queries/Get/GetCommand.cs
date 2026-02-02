using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.CourtCaseDates.Queries.Get;

public class GetCommand : IRequest<ErrorOr<IEnumerable<CourtCaseDateResult>>>
{
}
