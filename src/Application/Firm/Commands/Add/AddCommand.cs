using ErrorOr;
using MediatR;

namespace Application.Firm.Commands.Add;

public record AddCommand(
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
) : IRequest<ErrorOr<Guid>>;
