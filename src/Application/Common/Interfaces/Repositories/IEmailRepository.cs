namespace Application.Common.Interfaces.Repositories;

public interface IEmailRepository : IBaseRepository<Domain.Emails.Email>
{
    Task<Domain.Emails.Email?> GetUnsentEmails(CancellationToken cancellationToken);
}
