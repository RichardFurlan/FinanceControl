namespace FinanceControl.Domain.Events;

public class TransferCompletedEvent :  DomainEvent
{
    public Guid SourceAccountId { get; }
    public Guid DestinationAccountId { get; }
    public decimal Amount { get; }

    public TransferCompletedEvent(
        Guid sourceAccountId, 
        Guid destinationAccountId, 
        decimal amount)
    {
        SourceAccountId = sourceAccountId;
        DestinationAccountId = destinationAccountId;
        Amount = amount;
    }
}