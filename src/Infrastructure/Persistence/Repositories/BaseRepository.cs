using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Session;
using Domain.Common;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly IApplicationDBContext _context;
    protected readonly DbSet<T> _dbSet;
    private readonly ISessionResolver _sessionResolver;

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
            auditable.CreatedBy = _sessionResolver.UserId; // 👈 Optional if you track users
            auditable.LastModified = DateTime.UtcNow;
            auditable.LastModifiedBy = _sessionResolver.UserId;
            auditable.IsDeleted = false;
        }

        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public virtual Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (entity is AuditableEntity auditable)
        {
            auditable.LastModified = DateTime.UtcNow;
            auditable.LastModifiedBy = _sessionResolver.UserId;
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
            auditable.LastModifiedBy = _sessionResolver.UserId;

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
        return await _dbSet.FindAsync([id, new Guid(_sessionResolver.UserId!)], cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(x => EF.Property<Guid>(x, "UserId") == new Guid(_sessionResolver.UserId!)).ToListAsync(cancellationToken);
    }
}
