using Application.Common.Models;

namespace Application.Common.Interfaces.Services;

public interface IEmailService : IBaseService<EmailResult>
{
    Task<Domain.Emails.Email?> GetUnsentEmails(CancellationToken cancellationToken);
    Task SendEmails(Domain.Emails.Email email, CancellationToken cancellationToken);
}
