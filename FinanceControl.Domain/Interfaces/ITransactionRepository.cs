using FinanceControl.Domain.Entities;
using FinanceControl.Domain.Enums;

namespace FinanceControl.Domain.Interfaces;

public interface ITransactionRepository : IGenericRepository<Transaction>
{
    // Métodos ESPECÍFICOS de Transaction
    Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Transaction>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Transaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Transaction>> GetByTypeAsync(TransactionType type, Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Transaction>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task<decimal> GetTotalByTypeAsync(TransactionType type, Guid userId, CancellationToken cancellationToken = default);
}