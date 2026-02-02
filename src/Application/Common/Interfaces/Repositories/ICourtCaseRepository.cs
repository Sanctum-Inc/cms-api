namespace Application.Common.Interfaces.Repositories;

public interface ICourtCaseRepository : IBaseRepository<Domain.CourtCases.CourtCase>
{
    Task<Domain.CourtCases.CourtCase?> GetByCaseIdAsync(Guid guid1, Guid guid2, CancellationToken cancellationToken);
}
