using FinanceControl.Application.DTOs;
using MediatR;

namespace FinanceControl.Application.UseCases.Accounts.GetAccountById;

public record GetAccountByIdQuery(Guid Id) : IRequest<ResultViewModel<AccountDto>>;