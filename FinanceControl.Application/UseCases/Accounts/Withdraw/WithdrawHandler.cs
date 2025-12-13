using FinanceControl.Application.Common;
using FinanceControl.Application.DTOs;
using FinanceControl.Application.Interfaces;
using FinanceControl.Domain.Entities;
using FinanceControl.Domain.Interfaces;
using MediatR;

namespace FinanceControl.Application.UseCases.Accounts.Withdraw;

public class WithdrawHandler : IRequestHandler<WithdrawCommand, ResultViewModel<AccountDto>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICurrentUserService _currentUser;
    private readonly NotificationContext _notifications;

    public WithdrawHandler(IAccountRepository accountRepository, ICategoryRepository categoryRepository, ICurrentUserService currentUser, NotificationContext notifications)
    {
        _accountRepository = accountRepository;
        _categoryRepository = categoryRepository;
        _currentUser = currentUser;
        _notifications = notifications;
    }

    public async Task<ResultViewModel<AccountDto>> Handle(WithdrawCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;
        if (!userId.HasValue)
            return ResultViewModel<AccountDto>.Unauthorized("User not authenticated");

        // Buscar conta
        var account = await _accountRepository.GetByIdAsync(request.AccountId, cancellationToken);
        if (account == null)
            return ResultViewModel<AccountDto>.NotFound("Account not found");

        // Verificar propriedade
        if (account.UserId != userId.Value)
            return ResultViewModel<AccountDto>.Forbidden("You don't have access to this account");

        // Buscar categoria (se informada)
        Category? category = null;
        if (request.CategoryId.HasValue)
        {
            category = await _categoryRepository.GetByIdAsync(request.CategoryId.Value, cancellationToken);
            if (category == null)
                _notifications.AddNotification(nameof(request.CategoryId), "Category not found");
        }

        // Se tiver erros de validação, retorna
        if (_notifications.HasNotifications)
            return ResultViewModel<AccountDto>.ValidationError(_notifications.GetValidationErrors());

        // Executar saque (pode lançar DomainException se saldo insuficiente)
        account.Withdraw(request.Amount, request.Description, category, userId.Value);

        await _accountRepository.UpdateAsync(account, cancellationToken);

        return ResultViewModel<AccountDto>.Success(
            AccountDto.FromEntity(account),
            $"Withdrew {account.Currency} {request.Amount:N2} successfully"
        );
    }
}