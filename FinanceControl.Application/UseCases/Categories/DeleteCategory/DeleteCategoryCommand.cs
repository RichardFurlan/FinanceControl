using FinanceControl.Application.DTOs;
using MediatR;

namespace FinanceControl.Application.UseCases.Categories.DeleteCategory;

public record DeleteCategoryCommand(Guid Id) : IRequest<ResultViewModel>;