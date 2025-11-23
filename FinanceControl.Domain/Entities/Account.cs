using FinanceControl.Domain.Enums;
using FinanceControl.Domain.Events;
using FinanceControl.Domain.Exceptions;
using FinanceControl.Domain.ValueObjects;

namespace FinanceControl.Domain.Entities;

public class Account : EntityBase
{
    private readonly List<Transaction> _transactions = new();
    private readonly List<DomainEvent> _domainEvents = new();
    
    public string Name { get; private set; }
    public AccountType Type { get; private set; }
    public Money Balance { get; private set; }
    public string Currency { get; private set; }
    public Guid UserId { get; private set; }
    
    // Navigation properties
    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public User User { get; private set; } = null!;
    
    // EF Core Constructor
    private Account() 
    {
        Name = string.Empty;
        Currency = string.Empty;
        Balance = null!;
    }

    public Account(string name, AccountType type, string currency, Guid userId, decimal initialBalance = 0) 
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Account name cannot be empty");

        if (name.Length > 100)
            throw new DomainException("Account name cannot exceed 100 characters");
        
        Name = name;
        Type = type;
        Currency = currency.ToUpperInvariant();
        Balance = new Money(initialBalance, Currency);
        UserId = userId;
        
        SetCreatedBy(UserId);
        AddDomainEvent(new AccountCreatedEvent(Id, Name, Type));
    }

    public void Deposit(decimal amount, string description, Category category, Guid userId)
    {
        if (!IsActive)
            throw new DomainException("Cannot deposit to inactive account");

        if (amount <= 0)
            throw new DomainException("Deposit amount must be positive");

        var transaction = new Transaction(
            Id,
            TransactionType.Income,
            new Money(amount, Currency),
            description,
            category,
            userId
        );

        _transactions.Add(transaction);
        Balance = Balance.Add(amount);
        SetUpdatedAt(userId);

        AddDomainEvent(new TransactionCreatedEvent(transaction.Id, Id, TransactionType.Income, amount));
    }

    public void Withdraw(decimal amount, string description, Category category, Guid userId)
    {
        if (!IsActive)
            throw new DomainException("Cannot withdraw from inactive account");

        if (amount <= 0)
            throw new DomainException("Withdraw amount must be positive");

        if (Balance.Amount < amount)
            throw new DomainException("Insufficient balance");

        var transaction = new Transaction(
            Id,
            TransactionType.Expense,
            new Money(amount, Currency),
            description,
            category,
            userId
        );

        _transactions.Add(transaction);
        Balance = Balance.Subtract(amount);
        SetUpdatedAt(userId);

        AddDomainEvent(new TransactionCreatedEvent(transaction.Id, Id, TransactionType.Expense, amount));
    }

    public void Transfer(Account destinationAccount, decimal amount, string description, Guid userId)
    {
        if (!IsActive)
            throw new DomainException("Cannot transfer from inactive account");

        if (!destinationAccount.IsActive)
            throw new DomainException("Cannot transfer to inactive account");

        if (amount <= 0)
            throw new DomainException("Transfer amount must be positive");

        if (Balance.Amount < amount)
            throw new DomainException("Insufficient balance");

        if (Currency != destinationAccount.Currency)
            throw new DomainException("Cannot transfer between accounts with different currencies");

        // Debitar da conta origem
        var withdrawTransaction = new Transaction(
            Id,
            TransactionType.Transfer,
            new Money(amount, Currency),
            $"Transfer to {destinationAccount.Name}: {description}",
            null,
            destinationAccount.UserId
        );

        // Creditar na conta destino
        var depositTransaction = new Transaction(
            destinationAccount.Id,
            TransactionType.Transfer,
            new Money(amount, Currency),
            $"Transfer from {Name}: {description}",
            null,
            destinationAccount.UserId
        );

        _transactions.Add(withdrawTransaction);
        destinationAccount._transactions.Add(depositTransaction);

        Balance = Balance.Subtract(amount);
        destinationAccount.Balance = destinationAccount.Balance.Add(amount);

        SetUpdatedAt(userId);
        destinationAccount.UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new TransferCompletedEvent(Id, destinationAccount.Id, amount));
    }

    public void UpdateName(string newName, Guid? userId = null)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new DomainException("Account name cannot be empty");

        if (newName.Length > 100)
            throw new DomainException("Account name cannot exceed 100 characters");

        Name = newName;
        SetUpdatedAt(userId);
    }

    public override void Delete(Guid userId)
    {
        if (IsDeleted)
            throw new DomainException("Account is already deleted");
        
        if (!IsActive)
            throw new DomainException("Cannot delete an inactive account. Activate it first.");
        
        base.Delete(userId);    
    }
    
    public override void Restore(Guid userId)
    {
        if (!IsDeleted)
            throw new DomainException("Account is not deleted");

        base.Restore(userId);
    }

    public override void Activate(Guid userId)
    {
        if (IsDeleted)
            throw new DomainException("Cannot activate a deleted account. Restore it first.");

        if (IsActive)
            throw new DomainException("Account is already active");
        
        base.Activate(userId);
    }

    public override void Deactivate(Guid userId)
    {
        if (IsDeleted)
            throw new DomainException("Cannot deactivate a deleted account");
        
        if (!IsActive)
            throw new DomainException("Account is already inactive");

        base.Deactivate(userId);
    }

    public decimal GetBalanceAtDate(DateTime date)
    {
        return _transactions
            .Where(t => t.Date <= date)
            .Sum(t => t.Type == TransactionType.Income ? t.Amount.Amount : -t.Amount.Amount);
    }

    private void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    } 
}