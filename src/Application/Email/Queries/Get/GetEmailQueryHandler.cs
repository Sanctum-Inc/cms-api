using Application.Common.Interfaces.Services;
using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.Email.Queries.Get;

public class GetEmailQueryHandler : IRequestHandler<GetEmailQuery, ErrorOr<IEnumerable<EmailResult>>>
{
    private readonly IEmailService _emailService;

    public GetEmailQueryHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task<ErrorOr<IEnumerable<EmailResult>>> Handle(GetEmailQuery request, CancellationToken cancellationToken)
    {
        var result = await _emailService.Get(cancellationToken);

        return result;
    }
}
