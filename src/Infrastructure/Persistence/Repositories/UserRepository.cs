using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Session;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;
public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(IApplicationDBContext context, ISessionResolver sessionResolver) : base(context, sessionResolver) { }

    public async Task<User?> GetByEmail(string email, CancellationToken cancellationToken)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }
}
