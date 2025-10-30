using ErrorOr;
using MediatR;

namespace Application.CourtCaseDates.Commands.Update;
public class UpdateCommandHandler : IRequestHandler<UpdateCommand, ErrorOr<bool>>
{
    public Task<ErrorOr<bool>> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
