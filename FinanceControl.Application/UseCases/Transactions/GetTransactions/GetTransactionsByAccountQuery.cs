using FinanceControl.Application.DTOs;
using MediatR;

namespace FinanceControl.Application.UseCases.Transactions.GetTransactions;

public record GetTransactionsByAccountQuery(Guid AccountId) : IRequest<ResultViewModel<IEnumerable<TransactionDto>>>;