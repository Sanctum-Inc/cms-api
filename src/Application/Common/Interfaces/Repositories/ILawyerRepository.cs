namespace Application.Common.Interfaces.Repositories;

public interface ILawyerRepository : IBaseRepository<Domain.Lawyers.Lawyer>
{
    Task<Domain.Lawyers.Lawyer?> GetLawyerReportInformation(Guid id, CancellationToken cancellationToken);
}
