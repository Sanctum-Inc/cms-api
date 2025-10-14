using Application.Common.Models;
using Application.CourtCase.Queries.Get;

namespace Application.Common.Interfaces.Services;
public interface ICourtCaseService
{
    Task<GetCourtCaseResult> Get(CancellationToken cancellationToken);
    Task<CourtCaseResult> GetById(string id, CancellationToken cancellationToken);
}
