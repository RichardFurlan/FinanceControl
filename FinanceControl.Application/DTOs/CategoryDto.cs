using FinanceControl.Domain.Entities;

namespace FinanceControl.Application.DTOs;

public record CategoryDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Icon { get; init; } = string.Empty;
    public string Color { get; init; } = string.Empty;
    public bool IsActive { get; init; }

    public static CategoryDto FromEntity(Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Icon = category.Icon,
            Color = category.Color,
            IsActive = category.IsActive
        };
    }

    public static IEnumerable<CategoryDto> FromEntities(IEnumerable<Category> categories)
    {
        return categories.Select(FromEntity);
    }
}