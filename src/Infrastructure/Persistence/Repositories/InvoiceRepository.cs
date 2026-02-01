using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Session;
using Domain.CourtCases;
using Domain.Invoices;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;
public class InvoiceRepository : BaseRepository<Domain.Invoices.Invoice>, IInvoiceRepository
{
    public InvoiceRepository(IApplicationDBContext context, ISessionResolver sessionResolver) : base(context, sessionResolver) { }

    public override async Task<IEnumerable<Invoice>> GetAll(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Items)
            .Include(c => c.Case)
            .Where(x => x.UserId.ToString() == _sessionResolver.UserId)
            .ToListAsync(cancellationToken);
    }


    public override async Task<Invoice?> GetByIdAndUserIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Items)
             .FirstOrDefaultAsync(x =>
                     EF.Property<Guid>(x, "UserId") == new Guid(_sessionResolver.UserId!) &&
                     EF.Property<Guid>(x, "Id") == id,
                 cancellationToken)
             ;
    }

    public async Task<string> GetNewInvoiceNumber(CancellationToken cancellationToken)
    {
        var result = await _dbSet
            .OrderByDescending(x => x.InvoiceNumber)
             .FirstOrDefaultAsync(x =>
                     EF.Property<Guid>(x, "UserId") == new Guid(_sessionResolver.UserId!),
                 cancellationToken);

        return result?.InvoiceNumber ?? "INV-001";
    }
}
