using Domain.Lawyers;
using ErrorOr;
using MediatR;

namespace Application.Lawyer.Commands.Add;
public record AddCommand(
    string Email,
    string Name,
    string Surname,
    string MobileNumber,
    Speciality Specialty
) : IRequest<ErrorOr<Guid>>;
