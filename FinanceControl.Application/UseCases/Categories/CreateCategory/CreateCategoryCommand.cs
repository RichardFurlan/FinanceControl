using FinanceControl.Application.DTOs;
using MediatR;

namespace FinanceControl.Application.UseCases.Categories.CreateCategory;

public record CreateCategoryCommand(
    string Name,
    string Description,
    string Icon = "📁",
    string Color = "#6B7280"
) : IRequest<ResultViewModel<CategoryDto>>;