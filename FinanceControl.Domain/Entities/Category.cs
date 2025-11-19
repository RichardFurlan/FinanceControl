using FinanceControl.Domain.Exceptions;

namespace FinanceControl.Domain.Entities;

/// <summary>
/// Entity: Categoria de transação
/// </summary>
public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Icon { get; private set; }
    public string Color { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // EF Core Constructor
    private Category() 
    { 
        Name = string.Empty;
        Icon = string.Empty;
        Color = string.Empty;
    }

    public Category(string name, string icon = "📁", string color = "#6B7280")
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Category name cannot be empty");

        if (name.Length > 50)
            throw new DomainException("Category name cannot exceed 50 characters");

        Id = Guid.NewGuid();
        Name = name;
        Icon = icon;
        Color = color;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new DomainException("Category name cannot be empty");

        if (newName.Length > 50)
            throw new DomainException("Category name cannot exceed 50 characters");

        Name = newName;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateIcon(string newIcon)
    {
        Icon = newIcon;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateColor(string newColor)
    {
        Color = newColor;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new DomainException("Category is already inactive");

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        if (IsActive)
            throw new DomainException("Category is already active");

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
}