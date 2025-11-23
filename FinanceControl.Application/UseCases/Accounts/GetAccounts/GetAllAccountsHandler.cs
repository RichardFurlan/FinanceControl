using FinanceControl.Application.DTOs;
using FinanceControl.Application.Interfaces;
using FinanceControl.Domain.Interfaces;
using MediatR;

namespace FinanceControl.Application.UseCases.Accounts.GetAccounts;

public class GetAllAccountsHandler : IRequestHandler<GetAllAccountsQuery, ResultViewModel<IEnumerable<AccountDto>>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ICurrentUserService _currentUser;
    
    public GetAllAccountsHandler(IAccountRepository accountRepository, ICurrentUserService currentUser)
    {
        _accountRepository = accountRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultViewModel<IEnumerable<AccountDto>>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        if (!userId.HasValue)
        {
            return ResultViewModel<IEnumerable<AccountDto>>.Unauthorized("User not authenticated");
        }
        var accounts = await _accountRepository.GetByUserIdAsync(userId.Value, cancellationToken);

        var accountDtos = AccountDto.FromEntities(accounts);

        return ResultViewModel<IEnumerable<AccountDto>>.Success(accountDtos);
    }
}