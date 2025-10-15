using Application.Common.Interfaces.Repositories;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;
public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDBContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmail(string email, CancellationToken cancellationToken)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }
}
