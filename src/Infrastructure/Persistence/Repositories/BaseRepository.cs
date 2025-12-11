using System.Linq.Expressions;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Session;
using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly IApplicationDBContext _context;
    protected readonly DbSet<T> _dbSet;
    protected readonly ISessionResolver _sessionResolver;

    public BaseRepository(IApplicationDBContext context, ISessionResolver sessionResolver)
    {
        _context = context;
        _dbSet = _context.Set<T>();
        _sessionResolver = sessionResolver;
    }

    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync([id], cancellationToken);
    }

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
    }

    public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (entity is AuditableEntity auditable)
        {
            auditable.Created = DateTime.UtcNow;
            auditable.LastModified = DateTime.UtcNow;
            auditable.IsDeleted = false;

            if (!string.IsNullOrEmpty(_sessionResolver.UserId))
            {
                var userId = new Guid(_sessionResolver.UserId);
                auditable.CreatedBy = userId;
                auditable.LastModifiedBy = userId;
            }
            else
            {
                // Optionally set system or anonymous ID
                auditable.CreatedBy = Guid.Empty;
                auditable.LastModifiedBy = Guid.Empty;
            }
        }

        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public virtual Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (entity is AuditableEntity auditable)
        {
            auditable.LastModified = DateTime.UtcNow;
            auditable.LastModifiedBy = new Guid(_sessionResolver.UserId!);
        }

        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public virtual Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (entity is AuditableEntity auditable)
        {
            auditable.IsDeleted = true;
            auditable.LastModified = DateTime.UtcNow;
            auditable.LastModifiedBy = new Guid(_sessionResolver.UserId!);

            // 👇 If you want *soft delete*, update instead of removing
            _dbSet.Update(entity);
        }
        else
        {
            // hard delete fallback
            _dbSet.Remove(entity);
        }
        return Task.CompletedTask;
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(predicate, cancellationToken);
    }

    public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<T?> GetByIdAndUserIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(x =>
                    EF.Property<Guid>(x, "UserId") == new Guid(_sessionResolver.UserId!) &&
                    EF.Property<Guid>(x, "Id") == id,
                cancellationToken);
    }

    public virtual async Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(x => EF.Property<Guid>(x, "UserId") == new Guid(_sessionResolver.UserId!)).ToListAsync(cancellationToken);
    }
}
