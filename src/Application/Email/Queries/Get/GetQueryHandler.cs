using Application.Common.Interfaces.Services;
using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.Email.Queries.Get;

public class GetQueryHandler : IRequestHandler<GetQuery, ErrorOr<IEnumerable<EmailResult>>>
{
    private readonly IEmailService _emailService;

    public GetQueryHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task<ErrorOr<IEnumerable<EmailResult>>> Handle(GetQuery request, CancellationToken cancellationToken)
    {
        var result = await _emailService.Get(cancellationToken);

        return result;
    }
}
