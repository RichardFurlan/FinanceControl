using FinanceControl.Domain.Enums;
using FinanceControl.Domain.Exceptions;

namespace FinanceControl.Domain.Entities;

/// <summary>
/// Entity: Categoria de transação
/// </summary>
public class Category : EntityBase
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Icon { get; private set; }
    public string Color { get; private set; }
    public Guid UserId { get; private set; }
    
    // Navigation properties
    public User User { get; private set; } = null!;

    // EF Core Constructor
    private Category() 
    { 
        Name = string.Empty;
        Description = string.Empty;
        Icon = string.Empty;
        Color = string.Empty;
    }

    public Category(string name, string description, Guid userId, string icon = "📁", string color = "#6B7280") 
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Category name cannot be empty");

        if (name.Length > 50)
            throw new DomainException("Category name cannot exceed 50 characters");
        
        if (description.Length > 500)
            throw new DomainException("Category description cannot exceed 500 characters");
        
        Name = name;
        Description = description;
        Icon = icon;
        Color = color;
        
        SetCreatedBy(userId);
    }

    public void Update(string name,string description, string icon, string color = "#6B7280")
    {
        UpdateName(name);
        UpdateDescription(description);
        UpdateIcon(icon);
        UpdateColor(color);
    }

    private void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new DomainException("Category name cannot be empty");

        if (newName.Length > 50)
            throw new DomainException("Category name cannot exceed 50 characters");

        Name = newName;
        SetUpdatedAt(UserId);
    }

    private void UpdateDescription(string newDescription)
    {
        if (string.IsNullOrWhiteSpace(newDescription))
            throw new DomainException("Category description cannot be empty");

        if (newDescription.Length > 500)
            throw new DomainException("Category description cannot exceed 500 characters");

        Description = newDescription;
        SetUpdatedAt(UserId);
    }

    private void UpdateIcon(string newIcon)
    {
        Icon = newIcon;
        SetUpdatedAt(UserId);
    }

    private void UpdateColor(string newColor, Guid? userId = null)
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