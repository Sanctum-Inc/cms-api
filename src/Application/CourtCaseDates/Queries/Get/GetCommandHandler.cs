using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.CourtCaseDates.Queries.Get;
public class GetCommandHandler : IRequestHandler<GetCommand, ErrorOr<IEnumerable<CourtCaseDateResult>>>
{
    public Task<ErrorOr<IEnumerable<CourtCaseDateResult>>> Handle(GetCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
