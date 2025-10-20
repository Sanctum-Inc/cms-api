using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Session;

namespace Infrastructure.Persistence.Repositories;
public class InvoiceRepository : BaseRepository<Domain.Invoices.Invoice>, IInvoiceRepository
{
    public InvoiceRepository(IApplicationDBContext context, ISessionResolver sessionResolver) : base(context, sessionResolver) { }

}
