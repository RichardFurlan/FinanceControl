using FinanceControl.Application.DTOs;
using MediatR;

namespace FinanceControl.Application.UseCases.Accounts.UpdateAccount;

public record UpdateAccountCommand(
    Guid Id,
    string Name
) : IRequest<ResultViewModel<AccountDto>>;