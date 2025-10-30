using ErrorOr;
using MediatR;

namespace Application.CourtCaseDates.Commands.Delete;
public class DeleteCommandHandler : IRequestHandler<DeleteCommand, ErrorOr<bool>>
{
    public Task<ErrorOr<bool>> Handle(DeleteCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
