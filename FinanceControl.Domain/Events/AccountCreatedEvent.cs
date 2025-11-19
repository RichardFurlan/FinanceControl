using FinanceControl.Domain.Enums;

namespace FinanceControl.Domain.Events;

public class AccountCreatedEvent : DomainEvent
{
    public Guid AccountId { get; }
    public string Name { get; }
    public AccountType Type { get; }

    public AccountCreatedEvent(Guid accountId, string name, AccountType type)
    {
        AccountId = accountId;
        Name = name;
        Type = type;
    }
}