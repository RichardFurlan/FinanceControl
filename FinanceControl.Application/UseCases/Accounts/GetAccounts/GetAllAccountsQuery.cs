using FinanceControl.Application.DTOs;
using MediatR;

namespace FinanceControl.Application.UseCases.Accounts.GetAccounts;

public record GetAllAccountsQuery() : IRequest<ResultViewModel<IEnumerable<AccountDto>>>;