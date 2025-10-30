using ErrorOr;
using MediatR;

namespace Application.CourtCaseDates.Commands.Delete;
public class DeleteCommand : IRequest<ErrorOr<bool>>
{
}
