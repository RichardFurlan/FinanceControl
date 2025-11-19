namespace FinanceControl.Domain.Events;

public class DomainEvent
{
    public Guid Id { get; private set; }
    public DateTime OccurredOn { get; private set; }
    
    protected DomainEvent()
    {
        Id = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
    }
}