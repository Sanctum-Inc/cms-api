using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Email.Commands.Update;

public class UpdateCommandHandler : IRequestHandler<UpdateCommand, ErrorOr<bool>>
{
    private readonly IEmailService _emailService;

    public UpdateCommandHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task<ErrorOr<bool>> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        var result = await _emailService.Update(request, cancellationToken);

        return result;
    }
}
