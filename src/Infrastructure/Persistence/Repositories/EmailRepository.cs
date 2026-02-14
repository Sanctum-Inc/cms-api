using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Session;
using Domain.Emails;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class EmailRepository : BaseRepository<Email>, IEmailRepository
{
    public EmailRepository(IApplicationDBContext context, ISessionResolver sessionResolver) : base(context,
        sessionResolver)
    {
    }

    public async Task<Email?> GetUnsentEmails(CancellationToken cancellationToken)
    {
        return await _dbSet
            .FirstOrDefaultAsync(x => x.Status == EmailStatus.Pending, cancellationToken);
    }
}
