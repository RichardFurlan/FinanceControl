using FinanceControl.Application.DTOs;
using FinanceControl.Application.Interfaces;
using FinanceControl.Domain.Interfaces;
using MediatR;

namespace FinanceControl.Application.UseCases.Transactions.GetTransactions;

public class GetTransactionsByAccountHandler : IRequestHandler<GetTransactionsByAccountQuery, ResultViewModel<IEnumerable<TransactionDto>>>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ICurrentUserService _currentUser;

    public GetTransactionsByAccountHandler(ITransactionRepository transactionRepository, IAccountRepository accountRepository, ICurrentUserService currentUser)
    {
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultViewModel<IEnumerable<TransactionDto>>> Handle(GetTransactionsByAccountQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;
        if (!userId.HasValue)
            return ResultViewModel<IEnumerable<TransactionDto>>.Unauthorized("User not authenticated");

        // Verificar se a conta existe e pertence ao usuário
        var account = await _accountRepository.GetByIdAsync(request.AccountId, cancellationToken);
        if (account == null)
            return ResultViewModel<IEnumerable<TransactionDto>>.NotFound("Account not found");

        if (account.UserId != userId.Value)
            return ResultViewModel<IEnumerable<TransactionDto>>.Forbidden("You don't have access to this account");

        var transactions = await _transactionRepository.GetByAccountIdAsync(request.AccountId, cancellationToken);
        var transactionDtos = TransactionDto.FromEntities(transactions);

        return ResultViewModel<IEnumerable<TransactionDto>>.Success(transactionDtos);
    }
}