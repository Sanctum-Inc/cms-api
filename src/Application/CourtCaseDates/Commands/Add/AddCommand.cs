using ErrorOr;
using MediatR;

namespace Application.CourtCaseDates.Commands.Add;
public class AddCommand : IRequest<ErrorOr<bool>> 
{
}
