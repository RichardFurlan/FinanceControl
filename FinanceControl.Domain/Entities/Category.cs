using FinanceControl.Domain.Exceptions;

namespace FinanceControl.Domain.Entities;

/// <summary>
/// Entity: Categoria de transação
/// </summary>
public class Category : EntityBase
{
    public string Name { get; private set; }
    public string Icon { get; private set; }
    public string Color { get; private set; }

    // EF Core Constructor
    private Category() 
    { 
        Name = string.Empty;
        Icon = string.Empty;
        Color = string.Empty;
    }

    public Category(string name, Guid userId, string icon = "📁", string color = "#6B7280") 
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Category name cannot be empty");

        if (name.Length > 50)
            throw new DomainException("Category name cannot exceed 50 characters");
        Name = name;
        Icon = icon;
        Color = color;
        
        SetCreatedBy(userId);
    }

    public void UpdateName(string newName, Guid? userId = null)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new DomainException("Category name cannot be empty");

        if (newName.Length > 50)
            throw new DomainException("Category name cannot exceed 50 characters");

        Name = newName;
        SetUpdatedAt(userId);
    }

    public void UpdateIcon(string newIcon, Guid? userId = null)
    {
        Icon = newIcon;
        SetUpdatedAt(userId);
    }

    public void UpdateColor(string newColor, Guid? userId = null)
    {
        Color = newColor;
        SetUpdatedAt(userId);
    }

    public override void Deactivate(Guid userId)
    {
        if (IsDeleted)
            throw new DomainException("Cannot deactivate a deleted category");
        
        if (!IsActive)
            throw new DomainException("Category is already inactive");

        base.Deactivate(userId);
    }

    public override void Activate(Guid userId)
    {
        if (IsDeleted)
            throw new DomainException("Cannot activate a deleted category. Restore it first.");

        if (IsActive)
            throw new DomainException("Category is already active");

        base.Activate(userId);
    }
    
    public override void Delete(Guid userId)
    {
        if (IsDeleted)
            throw new DomainException("Category is already deleted");
        
        if (!IsActive)
            throw new DomainException("Cannot delete an inactive category. Activate it first.");
        
        base.Delete(userId);    
    }
    
    public override void Restore(Guid userId)
    {
        if (!IsDeleted)
            throw new DomainException("Category is not deleted");

        base.Restore(userId);
    }
}