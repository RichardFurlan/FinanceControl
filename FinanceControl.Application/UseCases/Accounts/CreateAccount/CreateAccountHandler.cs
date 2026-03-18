using FinanceControl.Application.Common;
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
    private readonly NotificationContext _notifications;
    
    public CreateAccountHandler(IAccountRepository accountRepository, ICurrentUserService currentUser, NotificationContext notifications)
    {
        _accountRepository = accountRepository;
        _currentUser = currentUser;
        _notifications = notifications;
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
            _notifications.AddNotification(nameof(request.Type), $"Type {request.Type} not recognised");
        }
        
        // Criar entidade de domínio
        
        var account = new Account(
            request.Name,
            accountType,
            request.Currency,
            userId.Value,
            initialBalance: request.InitialBalance
        );
        
        if (_notifications.HasNotifications)
            return ResultViewModel<AccountDto>.ValidationError(_notifications.GetValidationErrors());
        
        await _accountRepository.AddAsync(account, cancellationToken);
        
        // Retornar DTO
        return ResultViewModel<AccountDto>.Success(AccountDto.FromEntity(account));
    }
}