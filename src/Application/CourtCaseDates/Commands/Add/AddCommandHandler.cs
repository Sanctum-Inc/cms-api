using ErrorOr;
using MediatR;

namespace Application.CourtCaseDates.Commands.Add;
public class AddCommandHandler : IRequestHandler<AddCommand, ErrorOr<bool>>
{
    public Task<ErrorOr<bool>> Handle(AddCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
