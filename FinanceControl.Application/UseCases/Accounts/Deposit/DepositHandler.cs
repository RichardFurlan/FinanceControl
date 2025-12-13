using FinanceControl.Application.DTOs;
using FinanceControl.Application.Interfaces;
using FinanceControl.Domain.Entities;
using FinanceControl.Domain.Interfaces;
using MediatR;

namespace FinanceControl.Application.UseCases.Accounts.Deposit;

public class DepositHandler : IRequestHandler<DepositCommand, ResultViewModel<AccountDto>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICurrentUserService _currentUser;

    public DepositHandler(IAccountRepository accountRepository, ICategoryRepository categoryRepository, ICurrentUserService currentUser)
    {
        _accountRepository = accountRepository;
        _categoryRepository = categoryRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultViewModel<AccountDto>> Handle(DepositCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;
        if (!userId.HasValue)
            return ResultViewModel<AccountDto>.Unauthorized("User not authenticated");

        var account = await _accountRepository.GetByIdAsync(request.AccountId, cancellationToken);
        if (account == null)
            return ResultViewModel<AccountDto>.NotFound("Account not found");

        if (account.UserId != userId.Value)
            return ResultViewModel<AccountDto>.Forbidden("You don't have access to this account");

        Category? category = null;
        if (request.CategoryId.HasValue)
        {
            category = await _categoryRepository.GetByIdAsync(request.CategoryId.Value, cancellationToken);
        }

        account.Deposit(request.Amount, request.Description, category, userId.Value);
        await _accountRepository.UpdateAsync(account, cancellationToken);

        return ResultViewModel<AccountDto>.Success(
            AccountDto.FromEntity(account),
            $"Deposited {account.Currency} {request.Amount:N2} successfully"
        );
    }
}