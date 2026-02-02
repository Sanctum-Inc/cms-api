using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Session;
using Domain.Documents;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class DocumentRepository : BaseRepository<Document>, IDocumentRepository
{
    public DocumentRepository(IApplicationDBContext context, ISessionResolver sessionResolver) : base(context,
        sessionResolver)
    {
    }

    public Task<List<Document>> GetByUserIdAsync(Guid userId)
    {
        return _dbSet
            .AsNoTracking()
            .Where(d => d.UserId == userId)
            .ToListAsync();
    }
}
