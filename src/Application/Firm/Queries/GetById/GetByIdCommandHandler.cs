using Application.Common.Interfaces.Services;
using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.Firm.Queries.GetById;

public class GetByIdCommandHandler(
    IFirmService firmService)
    : IRequestHandler<GetByIdCommand, ErrorOr<FirmResult>>
{

    public async Task<ErrorOr<FirmResult>> Handle(GetByIdCommand request, CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty)
            return Error.Validation("Firm.Id", "Firm ID is required");

        var result = await firmService.GetById(request.Id, cancellationToken);

        return result;
    }
}
