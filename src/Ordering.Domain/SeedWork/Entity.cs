namespace Ordering.Domain.SeedWork;

public abstract class Entity
{
    public int Id { get; protected set; }
    public bool IsTransient() => Id == default;
}