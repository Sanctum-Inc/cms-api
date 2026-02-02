using Application.Common.Interfaces.Services;
using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.Email.Queries.GetById;

public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, ErrorOr<EmailResult>>
{
    private readonly IEmailService _emailService;

    public GetByIdQueryHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task<ErrorOr<EmailResult>> Handle(GetByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _emailService.GetById(request.Id, cancellationToken);

        return result;
    }
}
