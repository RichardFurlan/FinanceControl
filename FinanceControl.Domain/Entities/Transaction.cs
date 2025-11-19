using FinanceControl.Domain.Enums;
using FinanceControl.Domain.Exceptions;
using FinanceControl.Domain.ValueObjects;

namespace FinanceControl.Domain.Entities;

/// <summary>
/// Entity: Transação financeira
/// </summary>
public class Transaction
{
    public Guid Id { get; private set; }
    public Guid AccountId { get; private set; }
    public TransactionType Type { get; private set; }
    public Money Amount { get; private set; }
    public string Description { get; private set; }
    public DateTime Date { get; private set; }
    public Guid? CategoryId { get; private set; }
    public string? Notes { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Navigation properties
    public Account Account { get; private set; } = null!;
    public Category? Category { get; private set; }

    // EF Core Constructor
    private Transaction() 
    { 
        Description = string.Empty;
        Amount = null!;
    }

    public Transaction(
        Guid accountId,
        TransactionType type,
        Money amount,
        string description,
        Category? category,
        DateTime? date = null,
        string? notes = null)
    {
        if (amount.Amount <= 0)
            throw new DomainException("Transaction amount must be positive");

        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Transaction description cannot be empty");

        if (description.Length > 500)
            throw new DomainException("Transaction description cannot exceed 500 characters");

        Id = Guid.NewGuid();
        AccountId = accountId;
        Type = type;
        Amount = amount;
        Description = description;
        Date = date ?? DateTime.UtcNow;
        CategoryId = category?.Id;
        Category = category;
        Notes = notes;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateDescription(string newDescription)
    {
        if (string.IsNullOrWhiteSpace(newDescription))
            throw new DomainException("Transaction description cannot be empty");

        if (newDescription.Length > 500)
            throw new DomainException("Transaction description cannot exceed 500 characters");

        Description = newDescription;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateAmount(decimal newAmount)
    {
        if (newAmount <= 0)
            throw new DomainException("Transaction amount must be positive");

        Amount = new Money(newAmount, Amount.Currency);
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateCategory(Category? newCategory)
    {
        CategoryId = newCategory?.Id;
        Category = newCategory;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateNotes(string? notes)
    {
        Notes = notes;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsIncome() => Type == TransactionType.Income;
    public bool IsExpense() => Type == TransactionType.Expense;
    public bool IsTransfer() => Type == TransactionType.Transfer;
}