using FinanceControl.Application.DTOs;
using FinanceControl.Domain.Interfaces;
using MediatR;

namespace FinanceControl.Application.UseCases.Categories.GetCategories;

public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, ResultViewModel<IEnumerable<CategoryDto>>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetAllCategoriesHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<ResultViewModel<IEnumerable<CategoryDto>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetActiveAsync(cancellationToken);
        var categoryDtos = CategoryDto.FromEntities(categories);

        return ResultViewModel<IEnumerable<CategoryDto>>.Success(categoryDtos);
    }
}