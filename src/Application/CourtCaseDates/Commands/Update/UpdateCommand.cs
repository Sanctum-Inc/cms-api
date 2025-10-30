using ErrorOr;
using MediatR;

namespace Application.CourtCaseDates.Commands.Update;
public class UpdateCommand : IRequest<ErrorOr<bool>>
{
}
