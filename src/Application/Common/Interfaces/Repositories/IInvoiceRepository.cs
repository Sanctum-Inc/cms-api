namespace Application.Common.Interfaces.Repositories;

public interface IInvoiceRepository : IBaseRepository<Domain.Invoices.Invoice>
{
    Task<string> GetNewInvoiceNumber(CancellationToken cancellationToken);
}
