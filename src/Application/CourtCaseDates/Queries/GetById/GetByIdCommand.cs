using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.CourtCaseDates.Queries.GetById;
public class GetByIdCommand : IRequest<ErrorOr<CourtCaseDateResult?>>
{
}
