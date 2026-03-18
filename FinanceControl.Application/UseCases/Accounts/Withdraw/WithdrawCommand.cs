using FinanceControl.Application.DTOs;
using MediatR;

namespace FinanceControl.Application.UseCases.Accounts.Withdraw;


public record WithdrawCommand(
    Guid AccountId,
    decimal Amount,
    string Description,
    Guid? CategoryId
) : IRequest<ResultViewModel<AccountDto>>;