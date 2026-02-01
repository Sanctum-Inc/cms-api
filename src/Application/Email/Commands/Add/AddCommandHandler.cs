using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Email.Commands.Add;
public class AddCommandHandler : IRequestHandler<AddCommand, ErrorOr<Guid>>
{
    private readonly IEmailService _emailService;

    public AddCommandHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task<ErrorOr<Guid>> Handle(AddCommand request, CancellationToken cancellationToken)
    {
        var result = await _emailService.Add(request, cancellationToken);

        return result;
    }
}
