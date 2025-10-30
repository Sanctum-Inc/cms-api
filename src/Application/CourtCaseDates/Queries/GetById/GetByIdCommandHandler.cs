using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.CourtCaseDates.Queries.GetById;
public class GetByIdCommandHandler : IRequestHandler<GetByIdCommand, ErrorOr<CourtCaseDateResult?>>
{
    public Task<ErrorOr<CourtCaseDateResult?>> Handle(GetByIdCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
