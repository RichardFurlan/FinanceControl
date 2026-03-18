using FinanceControl.Application.DTOs;
using FinanceControl.Application.Interfaces;
using FinanceControl.Domain.Interfaces;
using MediatR;

namespace FinanceControl.Application.UseCases.Categories.DeleteCategory;

public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, ResultViewModel>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICurrentUserService _currentUser;

    public DeleteCategoryHandler(ICategoryRepository categoryRepository, ICurrentUserService currentUser)
    {
        _categoryRepository = categoryRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultViewModel> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;
        if (!userId.HasValue)
            return ResultViewModel.Unauthorized("User not authenticated");

        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (category == null)
            return ResultViewModel.NotFound("Category not found");

        if (category.UserId != userId.Value)
            return ResultViewModel.Forbidden("You don't have access to this category");

        category.Delete(request.Id);
        await _categoryRepository.UpdateAsync(category, cancellationToken);

        return ResultViewModel.Success("Category deleted successfully");
    }
}