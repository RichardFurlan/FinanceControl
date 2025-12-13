using FinanceControl.Application.Common;
using FinanceControl.Application.DTOs;
using FinanceControl.Application.Interfaces;
using FinanceControl.Domain.Entities;
using FinanceControl.Domain.Interfaces;
using MediatR;

namespace FinanceControl.Application.UseCases.Categories.CreateCategory;

public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, ResultViewModel<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICurrentUserService _currentUser;
    private readonly NotificationContext _notifications;

    public CreateCategoryHandler(ICategoryRepository categoryRepository, ICurrentUserService currentUser, NotificationContext notifications)
    {
        _categoryRepository = categoryRepository;
        _currentUser = currentUser;
        _notifications = notifications;
    }

    public async Task<ResultViewModel<CategoryDto>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;
        if (!userId.HasValue)
            return ResultViewModel<CategoryDto>.Unauthorized("User not authenticated");
        
        if (await _categoryRepository.ExistsByNameAsync(request.Name, cancellationToken))
            _notifications.AddNotification(nameof(request.Name), $"Name {request.Name} already exists");

        var category = new Category(request.Name,userId.Value, request.Icon, request.Color);

        if(_notifications.HasNotifications)
            return ResultViewModel<CategoryDto>.ValidationError(_notifications.GetValidationErrors());
        
        await _categoryRepository.AddAsync(category, cancellationToken);

        return ResultViewModel<CategoryDto>.Created(
            CategoryDto.FromEntity(category),
            "Category created successfully"
        );
    }
}