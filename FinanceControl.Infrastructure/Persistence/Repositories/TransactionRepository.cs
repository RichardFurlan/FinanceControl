using FinanceControl.Domain.Entities;
using FinanceControl.Domain.Enums;
using FinanceControl.Domain.Interfaces;
using FinanceControl.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.Infrastructure.Persistence.Repositories;

public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
{
    public TransactionRepository(FinanceDbContext context) : base(context)
    {
    }

    public override async Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Account)
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(t => t.AccountId == accountId)
            .OrderByDescending(t => t.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Transaction>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Account)
            .Include(t => t.Category)
            .Where(t => t.Account.UserId == userId)
            .OrderByDescending(t => t.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Transaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Account)
            .Include(t => t.Category)
            .Where(t => t.Account.UserId == userId && t.Date >= startDate && t.Date <= endDate)
            .OrderByDescending(t => t.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Transaction>> GetByTypeAsync(TransactionType type, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Account)
            .Include(t => t.Category)
            .Where(t => t.Account.UserId == userId && t.Type == type)
            .OrderByDescending(t => t.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Transaction>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Account)
            .Where(t => t.CategoryId == categoryId)
            .OrderByDescending(t => t.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<decimal> GetTotalByTypeAsync(TransactionType type, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Account)
            .Where(t => t.Account.UserId == userId && t.Type == type)
            .SumAsync(t => t.Amount.Amount, cancellationToken);
    }
}