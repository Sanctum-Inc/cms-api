using ErrorOr;
using MediatR;

namespace Application.InvoiceItem.Commands.Add;

public class AddCommand : IRequest<ErrorOr<Guid>>
{
    public required string InvoiceId { get; set; }
    public required string Name { get; set; }
    public required int Hours { get; set; }
    public required decimal CostPerHour { get; set; }
    public required string Date { get; set; }
    public required Guid CaseId { get; set; }
    public required string ClientName { get; set; }
    public required string Reference { get; set; }
}
