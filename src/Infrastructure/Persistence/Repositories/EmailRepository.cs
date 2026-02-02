using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Session;
using Domain.Emails;

namespace Infrastructure.Persistence.Repositories;

public class EmailRepository : BaseRepository<Email>, IEmailRepository
{
    public EmailRepository(IApplicationDBContext context, ISessionResolver sessionResolver) : base(context,
        sessionResolver)
    {
    }
}
