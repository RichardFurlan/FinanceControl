using FinanceControl.Application.DTOs;
using MediatR;

namespace FinanceControl.Application.UseCases.Accounts.Deposit;

public record DepositCommand(
    Guid AccountId,
    decimal Amount,
    string Description,
    Guid? CategoryId
) : IRequest<ResultViewModel<AccountDto>>;