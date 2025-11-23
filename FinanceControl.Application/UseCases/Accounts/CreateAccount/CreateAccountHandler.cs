using FinanceControl.Application.DTOs;
using FinanceControl.Application.Interfaces;
using FinanceControl.Domain.Entities;
using FinanceControl.Domain.Enums;
using FinanceControl.Domain.Interfaces;
using MediatR;

namespace FinanceControl.Application.UseCases.Accounts.CreateAccount;

public class CreateAccountHandler : IRequestHandler<CreateAccountCommand, ResultViewModel<AccountDto>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ICurrentUserService _currentUser;
    
    public CreateAccountHandler(IAccountRepository accountRepository, ICurrentUserService currentUser)
    {
        _accountRepository = accountRepository;
        _currentUser = currentUser;
    }


    public async Task<ResultViewModel<AccountDto>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;
        if (!userId.HasValue)
        {
            return ResultViewModel<AccountDto>.Unauthorized("User not authenticated");
        }
        
        if (!Enum.TryParse<AccountType>(request.Type, out var accountType))
        {
            throw new ArgumentException($"Invalid account type: {request.Type}");
        }
        
        // Criar entidade de domínio
        
        var account = new Account(
            request.Name,
            accountType,
            request.Currency,
            userId.Value,
            initialBalance: request.InitialBalance
        );
        
        await _accountRepository.AddAsync(account, cancellationToken);
        
        // Retornar DTO
        return ResultViewModel<AccountDto>.Success(AccountDto.FromEntity(account));
    }
}