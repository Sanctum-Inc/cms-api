using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Session;
using Domain.Emails;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class EmailRepository(IApplicationDBContext context, ISessionResolver sessionResolver)
    : BaseRepository<Email>(context,
        sessionResolver), IEmailRepository
{
    public async Task<Email?> GetUnsentEmails(CancellationToken cancellationToken)
    {
        return await _dbSet
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Status == EmailStatus.Pending, cancellationToken);
    }
}
