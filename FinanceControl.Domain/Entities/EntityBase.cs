namespace FinanceControl.Domain.Entities;

/// <summary>
/// Classe base para todas as entidades
/// Implementa soft delete e auditoria
/// </summary>
public abstract class EntityBase
{
    public Guid Id { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }
    public Guid CreatedByUserId { get; protected set; }
    public Guid UpdatedByUserId { get; protected set; }
    public bool IsActive { get; private set; }
    
    // Soft Delete
    public bool IsDeleted { get; protected set; }
    public DateTime? DeletedAt { get; protected set; }
    public Guid? DeletedByUserId { get; protected set; }
    
    protected EntityBase()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        IsDeleted = false;
        IsActive = true; 
    }

    protected void SetUpdatedAt(Guid? userId)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedByUserId = userId ?? UpdatedByUserId;
    }

    protected void SetCreatedBy(Guid userId)
    {
        CreatedByUserId = userId;
    }
    
    /// <summary>
    /// Marca a entidade como desativada 
    /// </summary>
    public virtual void Deactivate(Guid userId)
    {
        IsActive = false;
        SetUpdatedAt(userId);
    }
    
    /// <summary>
    /// Marca a entidade como ativada 
    /// </summary>
    public virtual void Activate(Guid userId)
    {
        IsActive = true;
        SetUpdatedAt(userId);
    }
    
    /// <summary>
    /// Marca a entidade como deletada (soft delete)
    /// </summary>
    public virtual void Delete(Guid userId)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedByUserId = userId;
    }

    /// <summary>
    /// Restaura uma entidade deletada
    /// </summary>
    public virtual void Restore(Guid userId)
    {
        IsActive = false;
        IsDeleted = false;
        DeletedAt = null;
        DeletedByUserId = null;
        SetUpdatedAt(userId);
    }
}