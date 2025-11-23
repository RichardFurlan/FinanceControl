using FinanceControl.Domain.Entities;
using FinanceControl.Domain.Interfaces;
using FinanceControl.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.Infrastructure.Persistence.Repositories;

public class AccountRepository : GenericRepository<Account>, IAccountRepository
{
    public AccountRepository(FinanceDbContext context) : base(context)
    {
    }
    
    public override async Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(a => a.Transactions)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Account>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(a => a.UserId == userId)
            .OrderBy(a => a.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Account>> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(a => a.UserId == userId && a.IsActive)
            .OrderBy(a => a.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Account>> GetDeletedByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .IgnoreQueryFilters()
            .Where(a => a.UserId == userId && a.IsDeleted)
            .OrderByDescending(a => a.DeletedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<decimal> GetTotalBalanceByUserIdAsync(Guid userId, string currency, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(a => a.UserId == userId && a.IsActive && a.Currency == currency)
            .SumAsync(a => a.Balance.Amount, cancellationToken);
    }

    public async Task<bool> HasActiveAccountsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(a => a.UserId == userId && a.IsActive, cancellationToken);
    }
}