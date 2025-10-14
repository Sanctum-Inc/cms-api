namespace Application.Common.Interfaces.Repositories;
public interface ICourtCaseRepository : IBaseRepository<Domain.CourtCases.CourtCase>
{
    Task<IEnumerable<Domain.CourtCases.CourtCase>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<Domain.CourtCases.CourtCase?> GetByIdAndUserIdAsync(Guid caseId, Guid userId, CancellationToken cancellationToken);
}