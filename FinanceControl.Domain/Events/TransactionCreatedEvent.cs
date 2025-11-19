using FinanceControl.Domain.Enums;

namespace FinanceControl.Domain.Events;

public class TransactionCreatedEvent : DomainEvent
{
    public Guid TransactionId { get; }
    public Guid AccountId { get; }
    public TransactionType Type { get; }
    public decimal Amount { get; }

    public TransactionCreatedEvent(
        Guid transactionId, 
        Guid accountId, 
        TransactionType type, 
        decimal amount)
    {
        TransactionId = transactionId;
        AccountId = accountId;
        Type = type;
        Amount = amount;
    }
}