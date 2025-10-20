using ErrorOr;
using MediatR;

namespace Application.Invoice.Commands.Update;
public record UpdateCommand(
       Guid Id,
       string InvoiceNumber,
       DateTime InvoiceDate,
       string ClientName,
       string Reference,
       string CaseName,
       string AccountName,
       string Bank,
       string BranchCode,
       string AccountNumber
   ) : IRequest<ErrorOr<bool>>;
