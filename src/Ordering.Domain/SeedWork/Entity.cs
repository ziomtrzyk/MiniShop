using MediatR;

namespace Ordering.Domain.SeedWork;

public abstract class Entity
{
    private readonly List<INotification> _domainEvents = [];
    public int Id { get; protected set; }
    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();
    public bool IsTransient() => Id == default;
    protected void AddDomainEvent(INotification domainEvent) => _domainEvents.Add(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();
}