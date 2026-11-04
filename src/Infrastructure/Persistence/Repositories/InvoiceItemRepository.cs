using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Session;
using Domain.Invoices;

namespace Infrastructure.Persistence.Repositories;

public class InvoiceItemRepository : BaseRepository<InvoiceItem>, IInvoiceItemRepository
{
    public InvoiceItemRepository(IApplicationDBContext context, ISessionResolver sessionResolver) : base(context,
        sessionResolver)
    {
    }
}
