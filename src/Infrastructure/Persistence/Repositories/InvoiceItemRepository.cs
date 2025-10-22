using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Session;
using Domain.InvoiceItems;

namespace Infrastructure.Persistence.Repositories;
public class InvoiceItemRepository : BaseRepository<Domain.InvoiceItems.InvoiceItem>, IInvoiceItemRepository
{
    public InvoiceItemRepository(IApplicationDBContext context, ISessionResolver sessionResolver) : base(context, sessionResolver) { }
}
