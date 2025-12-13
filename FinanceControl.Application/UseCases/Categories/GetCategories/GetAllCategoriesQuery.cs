using FinanceControl.Application.DTOs;
using MediatR;

namespace FinanceControl.Application.UseCases.Categories.GetCategories;

public record GetAllCategoriesQuery : IRequest<ResultViewModel<IEnumerable<CategoryDto>>>;