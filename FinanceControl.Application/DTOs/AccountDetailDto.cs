using FinanceControl.Domain.Entities;

namespace FinanceControl.Application.DTOs;

public record AccountDetailDto()
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public decimal Balance { get; init; }
    public string Currency { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    
    // Auditoria
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public Guid CreatedByUserId { get; init; }
    public Guid? UpdatedByUserId { get; init; }
    public bool IsDeleted { get; init; }
    public DateTime? DeletedAt { get; init; }

    public static AccountDetailDto FromEntity(Account account)
    {
        return new AccountDetailDto
        {
            Id = account.Id,
            Name = account.Name,
            Type = account.Type.ToString(),
            Balance = account.Balance.Amount,
            Currency = account.Currency,
            IsActive = account.IsActive,
            CreatedAt = account.CreatedAt,
            UpdatedAt = account.UpdatedAt,
            CreatedByUserId = account.CreatedByUserId,
            UpdatedByUserId = account.UpdatedByUserId,
            IsDeleted = account.IsDeleted,
            DeletedAt = account.DeletedAt
        };
    }
}