using Domain.CourtCases;
using Domain.Documents;
using Domain.InvoiceItems;

namespace Contracts.User.Responses;
public record UserResponse(
    Guid Id,
    string Email,
    string Name,
    string Surname,
    string MobileNumber,
    DateTime DateCreated);
