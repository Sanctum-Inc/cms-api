using ErrorOr;
using MediatR;

namespace Application.Invoice.Commands.Add;
public record AddCommand(
        string InvoiceNumber,
        DateTime InvoiceDate,
        string ClientName,
        string Reference,
        string AccountName,
        string Bank,
        string BranchCode,
        string AccountNumber,
        Guid CaseId
    ) : IRequest<ErrorOr<Guid>>;
