using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Session;

namespace Infrastructure.Persistence.Repositories;
public class EmailRepository : BaseRepository<Domain.Emails.Email>, IEmailRepository
{
    public EmailRepository(IApplicationDBContext context, ISessionResolver sessionResolver) : base(context, sessionResolver) { }
}
