using FinanceControl.Domain.Entities;

namespace FinanceControl.Domain.Interfaces;

public interface IAccountRepository : IGenericRepository<Account>
{
    Task<IEnumerable<Account>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Account>> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Account>> GetDeletedByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<decimal> GetTotalBalanceByUserIdAsync(Guid userId, string currency, CancellationToken cancellationToken = default);
    Task<bool> HasActiveAccountsAsync(Guid userId, CancellationToken cancellationToken = default);
}