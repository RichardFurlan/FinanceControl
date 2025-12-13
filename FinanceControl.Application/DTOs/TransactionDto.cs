using FinanceControl.Domain.Entities;

namespace FinanceControl.Application.DTOs;

public record TransactionDto
{
    public Guid Id { get; init; }
    public Guid AccountId { get; init; }
    public string Type { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public string Currency { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime Date { get; init; }
    public Guid? CategoryId { get; init; }
    public string? CategoryName { get; init; }
    public string? Notes { get; init; }

    public static TransactionDto FromEntity(Transaction transaction)
    {
        return new TransactionDto
        {
            Id = transaction.Id,
            AccountId = transaction.AccountId,
            Type = transaction.Type.ToString(),
            Amount = transaction.Amount.Amount,
            Currency = transaction.Amount.Currency,
            Description = transaction.Description,
            Date = transaction.Date,
            CategoryId = transaction.CategoryId,
            CategoryName = transaction.Category?.Name,
            Notes = transaction.Notes
        };
    }

    public static IEnumerable<TransactionDto> FromEntities(IEnumerable<Transaction> transactions)
    {
        return transactions.Select(FromEntity);
    }
}