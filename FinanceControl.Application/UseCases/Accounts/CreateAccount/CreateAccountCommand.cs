using FinanceControl.Application.DTOs;
using MediatR;

namespace FinanceControl.Application.UseCases.Accounts.CreateAccount;

public record CreateAccountCommand(
    string Name,
    string Type,
    string Currency,
    decimal InitialBalance = 0
    ) : IRequest<ResultViewModel<AccountDto>>;