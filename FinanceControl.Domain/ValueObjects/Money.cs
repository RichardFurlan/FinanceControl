using FinanceControl.Domain.Exceptions;

namespace FinanceControl.Domain.ValueObjects;
/// <summary>
/// Value Object: Representa dinheiro (valor + moeda)
/// Imutável e com lógica de validação
/// </summary>
public class Money : IEquatable<Money>
{
    public decimal Amount { get; }
    public string Currency { get; }
    
    // Construtor privado para EF Core
    private Money() 
    { 
        Currency = string.Empty;
    }
    
    public Money(decimal amount, string currency)
    {
        if (string.IsNullOrWhiteSpace(currency))
            throw new DomainException("Currency cannot be empty");

        if (currency.Length != 3)
            throw new DomainException("Currency must be a 3-letter ISO code (e.g., BRL, USD, EUR)");

        Amount = Math.Round(amount, 2);
        Currency = currency.ToUpperInvariant();
    }
    
    // Operações
    public Money Add(decimal amount) => new Money(Amount + amount, Currency);
    public Money Subtract(decimal amount) => new Money(Amount - amount, Currency);

    // Helpers
    public bool IsZero() => Amount == 0;
    public bool IsPositive() => Amount > 0;
    public bool IsNegative() => Amount < 0;
    
    public override string ToString() => $"{Currency} {Amount:N2}";
    
    // Igualdade (Value Object)
    public bool Equals(Money? other)
    {
        if (other is null) return false;
        return Amount == other.Amount && Currency == other.Currency;
    }
    
    public override bool Equals(object? obj) => obj is Money other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(Amount, Currency);
    
    // Factory Method
    public static Money FromAmount(decimal amount, string currency) => new Money(amount, currency);
}