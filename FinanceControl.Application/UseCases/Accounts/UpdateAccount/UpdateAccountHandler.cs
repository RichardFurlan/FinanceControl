using FinanceControl.Application.DTOs;
using FinanceControl.Application.Interfaces;
using FinanceControl.Domain.Interfaces;
using MediatR;

namespace FinanceControl.Application.UseCases.Accounts.UpdateAccount;

public class UpdateAccountHandler : IRequestHandler<UpdateAccountCommand, ResultViewModel<AccountDto>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ICurrentUserService _currentUser;

    public UpdateAccountHandler(IAccountRepository accountRepository, ICurrentUserService currentUser)
    {
        _accountRepository = accountRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultViewModel<AccountDto>> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;
        if (!userId.HasValue)
            return ResultViewModel<AccountDto>.Unauthorized("User not authenticated");
        
        var account = await _accountRepository.GetByIdAsync(request.Id, cancellationToken);
        if (account == null)
            return ResultViewModel<AccountDto>.NotFound("Account not found");

        if (account.UserId != userId.Value)
            return ResultViewModel<AccountDto>.Forbidden("You don't have access to this account");

        account.UpdateName(request.Name);
        await _accountRepository.UpdateAsync(account, cancellationToken);

        return ResultViewModel<AccountDto>.Success(
            AccountDto.FromEntity(account),
            "Account updated successfully"
        );
    }
}