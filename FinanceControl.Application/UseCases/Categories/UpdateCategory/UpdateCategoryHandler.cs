using FinanceControl.Application.DTOs;
using FinanceControl.Application.Interfaces;
using FinanceControl.Domain.Interfaces;
using MediatR;

namespace FinanceControl.Application.UseCases.Categories.UpdateCategory;

public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, ResultViewModel<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICurrentUserService _currentUser;

    public UpdateCategoryHandler(ICategoryRepository categoryRepository, ICurrentUserService currentUser)
    {
        _categoryRepository = categoryRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultViewModel<CategoryDto>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;
        if (!userId.HasValue)
            return ResultViewModel<CategoryDto>.Unauthorized("User not authenticated");

        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (category == null)
            return ResultViewModel<CategoryDto>.NotFound("Category not found");

        if (category.UserId != userId.Value)
            return ResultViewModel<CategoryDto>.Forbidden("You don't have access to this category");

        category.Update(request.Name, request.Description, request.Icon, request.Color);
        await _categoryRepository.UpdateAsync(category, cancellationToken);

        return ResultViewModel<CategoryDto>.Success(
            CategoryDto.FromEntity(category),
            "Category updated successfully"
        );
    }
}