using MediatR;
using ErrorOr;

namespace Application.Lawyer.Commands.Update;

public class UpdateCommand : IRequest<ErrorOr<bool>>
{
    public Guid Id { get; set; }
    public string Email { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Surname { get; set; } = default!;
    public string MobileNumber { get; set; } = default!;
    public int Specialty { get; set; }
}
