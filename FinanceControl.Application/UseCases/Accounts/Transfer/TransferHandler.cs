using FinanceControl.Application.DTOs;
using FinanceControl.Application.Interfaces;
using FinanceControl.Domain.Interfaces;
using MediatR;

namespace FinanceControl.Application.UseCases.Accounts.Transfer;

public class TransferHandler : IRequestHandler<TransferCommand, ResultViewModel>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ICurrentUserService _currentUser;

    public TransferHandler(IAccountRepository accountRepository, ICurrentUserService currentUser)
    {
        _accountRepository = accountRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultViewModel> Handle(TransferCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;
        if (!userId.HasValue)
            return ResultViewModel.Unauthorized("User not authenticated");

        var sourceAccount = await _accountRepository.GetByIdAsync(request.SourceAccountId, cancellationToken);
        if (sourceAccount == null)
            return ResultViewModel.NotFound("Source account not found");

        var destinationAccount = await _accountRepository.GetByIdAsync(request.DestinationAccountId, cancellationToken);
        if (destinationAccount == null)
            return ResultViewModel.NotFound("Destination account not found");

        if (sourceAccount.UserId != userId.Value || destinationAccount.UserId != userId.Value)
            return ResultViewModel.Forbidden("You don't have access to these accounts");

        sourceAccount.Transfer(destinationAccount, request.Amount, request.Description, userId.Value);

        await _accountRepository.UpdateAsync(sourceAccount, cancellationToken);
        await _accountRepository.UpdateAsync(destinationAccount, cancellationToken);

        return ResultViewModel.Success($"Transferred {sourceAccount.Currency} {request.Amount:N2} successfully");
    }
}