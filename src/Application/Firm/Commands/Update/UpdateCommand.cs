using ErrorOr;
using MediatR;

namespace Application.Firm.Commands.Update;

public record UpdateCommand(
    Guid Id,
    string Name,
    string Address,
    string Telephone,
    string Fax,
    string Mobile,
    string Email,
    string AttorneyAdmissionDate,
    string AdvocateAdmissionDate,
    string AccountName,
    string Bank,
    string BranchCode,
    string AccountNumber
) : IRequest<ErrorOr<bool>>;
