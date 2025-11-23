using FinanceControl.Domain.Entities;

namespace FinanceControl.Application.DTOs;

public record AccountDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public decimal Balance { get; init; }
    public string Currency { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    
    /// <summary>
    /// Converte de Entity para DTO
    /// </summary>
    public static AccountDto FromEntity(Account account)
    {
        return new AccountDto
        {
            Id = account.Id,
            Name = account.Name,
            Type = account.Type.ToString(),
            Balance = account.Balance.Amount,
            Currency = account.Currency,
            IsActive = account.IsActive,
        };
    }

    /// <summary>
    /// Converte lista de Entities para lista de DTOs
    /// </summary>
    public static IEnumerable<AccountDto> FromEntities(IEnumerable<Account> accounts)
    {
        return accounts.Select(FromEntity);
    }
}