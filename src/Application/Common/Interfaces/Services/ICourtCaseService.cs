using Application.Common.Models;
using Application.CourtCase.Commands.Add;
using Application.CourtCase.Commands.Update;
using Application.CourtCase.Queries.Get;
using ErrorOr;

namespace Application.Common.Interfaces.Services;
public interface ICourtCaseService
{
    Task<ErrorOr<bool>> Delete(string id, CancellationToken cancellationToken);
    Task<GetCourtCaseResult> Get(CancellationToken cancellationToken);
    Task<CourtCaseResult> GetById(string id, CancellationToken cancellationToken);
    Task<ErrorOr<bool>> Update(UpdateCommand request, CancellationToken cancellationToken);
    Task<ErrorOr<bool>> Add(AddCommand request, CancellationToken cancellationToken);
}
