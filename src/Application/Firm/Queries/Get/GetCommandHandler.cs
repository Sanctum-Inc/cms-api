using Application.Common.Interfaces.Services;
using Application.Common.Interfaces.Session;
using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.Firm.Queries.Get;

public class GetCommandHandler(
    IFirmService firmService,
    ISessionResolver sessionResolver)
    : IRequestHandler<GetCommand, ErrorOr<FirmResult>>
{
    public async Task<ErrorOr<FirmResult>> Handle(GetCommand request, CancellationToken cancellationToken)
    {
        if (sessionResolver.FirmId is null)
            return Error.Forbidden("Firm.Id", "Firm ID is required");

        var result = await firmService.GetById(new Guid(sessionResolver.FirmId), cancellationToken);

        return result;
    }
}
