using Domain.InvoiceItems;

namespace Application.Users.Queries;
public class UserResult
{
    public required Guid Id { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string MobileNumber { get; set; }
    public required DateTime Created { get; set; }
}
