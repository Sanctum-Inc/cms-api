using Application.Common.Models;
using ErrorOr;

namespace Application.Common.Interfaces.Services;

/// <summary>
///     Provides a set of operations for managing lawyer records.
/// </summary>
public interface ILawyerService : IBaseService<LawyerResult>
{
    Task<ErrorOr<LawyerReportResult>> GetLawyerReport(Guid requestLawyerId, CancellationToken cancellationToken);
}
