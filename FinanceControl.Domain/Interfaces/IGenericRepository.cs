using FinanceControl.Domain.Entities;

namespace FinanceControl.Domain.Interfaces;
/// <summary>
/// Repositório genérico com operações CRUD padrão
/// </summary>
public interface IGenericRepository<TEntity> where TEntity : EntityBase
{
    // Read
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<TEntity?> GetByIdIncludingDeletedAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> GetAllIncludingDeletedAsync(CancellationToken cancellationToken = default);
    
    // Create
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    
    // Update
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    
    // Delete (Soft)
    Task SoftDeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task SoftDeleteRangeAsync(IEnumerable<Guid> ids, Guid userId, CancellationToken cancellationToken = default);
    
    // Restore
    Task RestoreAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task RestoreRangeAsync(IEnumerable<Guid> ids, Guid userId, CancellationToken cancellationToken = default);
    
    // Delete (Hard) - Apenas para Admin
    Task HardDeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task HardDeleteRangeAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
    
    // Utilities
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<int> CountAsync(CancellationToken cancellationToken = default);
    Task<int> CountIncludingDeletedAsync(CancellationToken cancellationToken = default);

}