using Application.Common.Interfaces.Services;
using Application.Common.Interfaces.Session;
using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.Firm.Queries.Get;
public class GetCommandHandler : IRequestHandler<GetCommand, ErrorOr<FirmResult>>
{
    private readonly IFirmService _firmService;
    private readonly ISessionResolver _sessionResolver;

    public GetCommandHandler(
        IFirmService firmService,
        ISessionResolver sessionResolver)
    {
        _firmService = firmService;
        _sessionResolver = sessionResolver;
    }

    public async Task<ErrorOr<FirmResult>> Handle(GetCommand request, CancellationToken cancellationToken)
    {
        var result = await _firmService.GetById(new Guid(_sessionResolver.FirmId ?? ""), cancellationToken);

        return result;
    }
}
