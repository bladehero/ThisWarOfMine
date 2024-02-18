using CSharpFunctionalExtensions;

namespace ThisWarOfMine.Domain.Abstraction;

public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot
    where TKey : IComparable<TKey>
{
    private readonly List<IBaseDomainEvent> _changes = new();

    public bool HasChanges => _changes.Any();

    public IReadOnlyCollection<IBaseDomainEvent> GetUncommittedChanges() => _changes.AsReadOnly();

    public UnitResult<Error> Load(IEnumerable<IBaseDomainEvent> history)
    {
        ArgumentNullException.ThrowIfNull(history);

        return history.Select(@event => ApplyChange(@event, false)).Combine();
    }

    public void Commit() => _changes.Clear();

    protected UnitResult<Error> ApplyChange<TEvent>(TEvent @event, bool isNew = true)
        where TEvent : IBaseDomainEvent
    {
        ArgumentNullException.ThrowIfNull(@event);

        return Router.Route(@event).TapIf(isNew, () => _changes.Add(@event));
    }

    protected Result<TValue, Error> ApplyChange<TEvent, TValue>(TEvent @event, bool isNew = true)
        where TEvent : IBaseDomainEvent
    {
        ArgumentNullException.ThrowIfNull(@event);

        return Router.Route<TEvent, TValue>(@event).TapIf(isNew, () => _changes.Add(@event));
    }
}
