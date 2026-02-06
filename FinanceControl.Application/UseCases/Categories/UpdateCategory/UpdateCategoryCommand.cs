using FinanceControl.Application.DTOs;
using FinanceControl.Domain.Enums;
using MediatR;

namespace FinanceControl.Application.UseCases.Categories.UpdateCategory;

public record UpdateCategoryCommand(
    Guid Id,
    string Name,
    string Description,
    string Color,
    string Icon
) : IRequest<ResultViewModel<CategoryDto>>;