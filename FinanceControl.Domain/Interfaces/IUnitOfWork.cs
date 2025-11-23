using FinanceControl.Domain.Enums;

namespace FinanceControl.Domain.Interfaces;

public interface IUnitOfWork
{
    IAccountRepository Account { get; }
    ITransactionRepository Transaction { get; }
    ICategoryRepository Category { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}