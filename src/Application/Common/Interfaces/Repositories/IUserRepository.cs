using Domain.Users;

namespace Application.Common.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<User?> GetUserReportInformation(Guid userId, CancellationToken cancellationToken);
}
