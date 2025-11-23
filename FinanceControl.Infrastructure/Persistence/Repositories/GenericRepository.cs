using FinanceControl.Domain.Entities;
using FinanceControl.Domain.Interfaces;
using FinanceControl.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.Infrastructure.Persistence.Repositories;
/// <summary>
/// Implementação genérica do repositório
/// </summary>
public class GenericRepository<TEntity> : IGenericRepository<TEntity> 
    where TEntity : EntityBase
{
    protected readonly FinanceDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public GenericRepository(FinanceDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    #region Read

    public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
    }

    public virtual async Task<TEntity?> GetByIdIncludingDeletedAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllIncludingDeletedAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .IgnoreQueryFilters()
            .ToListAsync(cancellationToken);
    }

    #endregion

    #region Create

    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public virtual async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        var entitiesList = entities.ToList();
        await _dbSet.AddRangeAsync(entitiesList, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entitiesList;
    }

    #endregion

    #region Update

    public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        _dbSet.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region Soft Delete

    public virtual async Task SoftDeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity == null)
            throw new KeyNotFoundException($"{typeof(TEntity).Name} with ID {id} not found");

        entity.Delete(userId);
        await UpdateAsync(entity, cancellationToken);
    }

    public virtual async Task SoftDeleteRangeAsync(IEnumerable<Guid> ids, Guid userId, CancellationToken cancellationToken = default)
    {
        var entities = await _dbSet
            .Where(e => ids.Contains(e.Id))
            .ToListAsync(cancellationToken);

        foreach (var entity in entities)
        {
            entity.Delete(userId);
        }

        await UpdateRangeAsync(entities, cancellationToken);
    }

    #endregion

    #region Restore

    public virtual async Task RestoreAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdIncludingDeletedAsync(id, cancellationToken);
        if (entity == null)
            throw new KeyNotFoundException($"{typeof(TEntity).Name} with ID {id} not found");

        entity.Restore(userId);
        await UpdateAsync(entity, cancellationToken);
    }

    public virtual async Task RestoreRangeAsync(IEnumerable<Guid> ids, Guid userId, CancellationToken cancellationToken = default)
    {
        var entities = await _dbSet
            .IgnoreQueryFilters()
            .Where(e => ids.Contains(e.Id) && e.IsDeleted)
            .ToListAsync(cancellationToken);

        foreach (var entity in entities)
        {
            entity.Restore(userId);
        }

        await UpdateRangeAsync(entities, cancellationToken);
    }

    #endregion

    #region Hard Delete

    public virtual async Task HardDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdIncludingDeletedAsync(id, cancellationToken);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public virtual async Task HardDeleteRangeAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        var entities = await _dbSet
            .IgnoreQueryFilters()
            .Where(e => ids.Contains(e.Id))
            .ToListAsync(cancellationToken);

        _dbSet.RemoveRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region Utilities

    public virtual async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(e => e.Id == id, cancellationToken);
    }

    public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.CountAsync(cancellationToken);
    }

    public virtual async Task<int> CountIncludingDeletedAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .IgnoreQueryFilters()
            .CountAsync(cancellationToken);
    }

    #endregion
}