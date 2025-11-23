using FinanceControl.Domain.Enums;
using FinanceControl.Domain.Exceptions;
using FinanceControl.Domain.ValueObjects;

namespace FinanceControl.Domain.Entities;

/// <summary>
/// Entity: Transação financeira
/// </summary>
public class Transaction : EntityBase
{
    public Guid AccountId { get; private set; }
    public TransactionType Type { get; private set; }
    public Money Amount { get; private set; }
    public string Description { get; private set; }
    public DateTime Date { get; private set; }
    public Guid? CategoryId { get; private set; }
    public string? Notes { get; private set; }

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
        Guid userId,
        DateTime? date = null,
        string? notes = null
        )
    {
        if (amount.Amount <= 0)
            throw new DomainException("Transaction amount must be positive");

        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Transaction description cannot be empty");

        if (description.Length > 500)
            throw new DomainException("Transaction description cannot exceed 500 characters");
        
        AccountId = accountId;
        Type = type;
        Amount = amount;
        Description = description;
        Date = date ?? DateTime.UtcNow;
        CategoryId = category?.Id;
        Category = category;
        Notes = notes;
        
        SetCreatedBy(userId);
    }

    public void UpdateDescription(string newDescription, Guid? userId = null)
    {
        if (string.IsNullOrWhiteSpace(newDescription))
            throw new DomainException("Transaction description cannot be empty");

        if (newDescription.Length > 500)
            throw new DomainException("Transaction description cannot exceed 500 characters");

        Description = newDescription;
        SetUpdatedAt(userId);
    }

    public void UpdateAmount(decimal newAmount, Guid? userId = null)
    {
        if (newAmount <= 0)
            throw new DomainException("Transaction amount must be positive");

        Amount = new Money(newAmount, Amount.Currency);
        SetUpdatedAt(userId);
    }

    public void UpdateCategory(Category? newCategory, Guid? userId = null)
    {
        CategoryId = newCategory?.Id;
        Category = newCategory;
        SetUpdatedAt(userId);
    }

    public void UpdateNotes(string? notes,  Guid? userId = null)
    {
        Notes = notes;
        SetUpdatedAt(userId);
    }
    
    public override void Deactivate(Guid userId)
    {
        if (IsDeleted)
            throw new DomainException("Cannot deactivate a deleted transaction");
        
        if (!IsActive)
            throw new DomainException("Transaction is already inactive");

        base.Deactivate(userId);
    }

    public override void Activate(Guid userId)
    {
        if (IsDeleted)
            throw new DomainException("Cannot activate a deleted transaction. Restore it first.");

        if (IsActive)
            throw new DomainException("Transaction is already active");

        base.Activate(userId);
    }

    public override void Delete(Guid userId)
    {
        if (IsDeleted)
            throw new DomainException("Transaction is already deleted");
        
        if (!IsActive)
            throw new DomainException("Cannot delete an inactive transaction. Activate it first.");
        
        base.Delete(userId);    
    }
    
    public override void Restore(Guid userId)
    {
        if (!IsDeleted)
            throw new DomainException("Transaction is not deleted");

        base.Restore(userId);
    }
    
    public bool IsIncome() => Type == TransactionType.Income;
    public bool IsExpense() => Type == TransactionType.Expense;
    public bool IsTransfer() => Type == TransactionType.Transfer;
}