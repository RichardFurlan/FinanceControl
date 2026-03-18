using FluentValidation;

namespace FinanceControl.Application.UseCases.Categories.CreateCategory;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name is required")
            .MaximumLength(50).WithMessage("Category name cannot exceed 50 characters");

        RuleFor(x => x.Icon)
            .MaximumLength(10).WithMessage("Icon cannot exceed 10 characters");

        RuleFor(x => x.Color)
            .Matches("^#[0-9A-Fa-f]{6}$").WithMessage("Color must be a valid hex color (e.g., #6B7280)");
    }
}