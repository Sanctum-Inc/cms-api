using Application.Common.Models;
using ErrorOr;

namespace Application.Common.Interfaces.Services;

/// <summary>
///     Provides a set of operations for managing and retrieving court case data.
/// </summary>
public interface ICourtCaseService : IBaseService<CourtCaseResult>
{
    Task<ErrorOr<IEnumerable<CourtCaseNumberResult>?>> GetCaseNumbers(CancellationToken cancellationToken);
    Task<ErrorOr<CourtCaseInformationResult>> GetCourtCaseInformationById(Guid id, CancellationToken cancellationToken);
}
