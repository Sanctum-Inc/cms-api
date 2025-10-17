
using Domain.Documents;

namespace Application.Common.Interfaces.Repositories;
public interface IDocumentRepository : IBaseRepository<Domain.Documents.Document>
{
    Task<Domain.Documents.Document?> GetByIdAndDocumentIdAsync(Guid id, Guid userId, CancellationToken cancellationToken);
    Task<List<Domain.Documents.Document>> GetByUserIdAsync(Guid guid);
}
