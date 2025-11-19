using FinanceControl.Domain.Enums;
using FinanceControl.Domain.Events;
using FinanceControl.Domain.Exceptions;
using FinanceControl.Domain.ValueObjects;

namespace FinanceControl.Domain.Entities;

public class Account
{
   private readonly List<Transaction> _transactions = new();
    private readonly List<DomainEvent> _domainEvents = new();

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public AccountType Type { get; private set; }
    public Money Balance { get; private set; }
    public string Currency { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    
    // Navigation properties
    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    // EF Core Constructor
    private Account() { }

    public Account(string name, AccountType type, string currency, decimal initialBalance = 0)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Account name cannot be empty");

        if (name.Length > 100)
            throw new DomainException("Account name cannot exceed 100 characters");

        Id = Guid.NewGuid();
        Name = name;
        Type = type;
        Currency = currency.ToUpperInvariant();
        Balance = new Money(initialBalance, Currency);
        IsActive = true;
        CreatedAt = DateTime.UtcNow;

        AddDomainEvent(new AccountCreatedEvent(Id, Name, Type));
    }

    public void Deposit(decimal amount, string description, Category category)
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
            category
        );

        _transactions.Add(transaction);
        Balance = Balance.Add(amount);
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new TransactionCreatedEvent(transaction.Id, Id, TransactionType.Income, amount));
    }

    public void Withdraw(decimal amount, string description, Category category)
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
            category
        );

        _transactions.Add(transaction);
        Balance = Balance.Subtract(amount);
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new TransactionCreatedEvent(transaction.Id, Id, TransactionType.Expense, amount));
    }

    public void Transfer(Account destinationAccount, decimal amount, string description)
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
            null
        );

        // Creditar na conta destino
        var depositTransaction = new Transaction(
            destinationAccount.Id,
            TransactionType.Transfer,
            new Money(amount, Currency),
            $"Transfer from {Name}: {description}",
            null
        );

        _transactions.Add(withdrawTransaction);
        destinationAccount._transactions.Add(depositTransaction);

        Balance = Balance.Subtract(amount);
        destinationAccount.Balance = destinationAccount.Balance.Add(amount);

        UpdatedAt = DateTime.UtcNow;
        destinationAccount.UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new TransferCompletedEvent(Id, destinationAccount.Id, amount));
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new DomainException("Account name cannot be empty");

        if (newName.Length > 100)
            throw new DomainException("Account name cannot exceed 100 characters");

        Name = newName;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        if (IsActive)
            throw new DomainException("Account is already active");

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new DomainException("Account is already inactive");

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
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