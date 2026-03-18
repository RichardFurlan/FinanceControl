using FinanceControl.Application.DTOs;
using FinanceControl.Application.Interfaces;
using FinanceControl.Domain.Interfaces;
using MediatR;

namespace FinanceControl.Application.UseCases.Accounts.GetAccountById;

public class GetAccountByIdHandler : IRequestHandler<GetAccountByIdQuery, ResultViewModel<AccountDto>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ICurrentUserService _currentUser;

    public GetAccountByIdHandler(IAccountRepository accountRepository, ICurrentUserService currentUser)
    {
        _accountRepository = accountRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultViewModel<AccountDto>> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;
        if (!userId.HasValue)
            return ResultViewModel<AccountDto>.Unauthorized("User not authenticated");
        
        var account = await _accountRepository.GetByIdAsync(request.Id, cancellationToken);
        if (account == null)
            return ResultViewModel<AccountDto>.NotFound("Account not found");

        if (account.UserId != userId.Value)
            return ResultViewModel<AccountDto>.Forbidden("You don't have access to this account");

        return ResultViewModel<AccountDto>.Success(AccountDto.FromEntity(account));
    }
}