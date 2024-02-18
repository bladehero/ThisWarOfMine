using CSharpFunctionalExtensions;

namespace ThisWarOfMine.Domain.Abstraction;

public sealed class InstanceEventRouter
{
    private readonly Dictionary<Key, Func<object, Result<object, Error>>> _handlers = new();

    internal InstanceEventRouter() { }

    internal void Configure<TEvent, TError>(DomainEventHandler<TEvent, TError> handler)
        where TEvent : IBaseDomainEvent
        where TError : Error
    {
        ArgumentNullException.ThrowIfNull(handler);

        _handlers.Add(Key.From<TEvent>(), @event => handler((TEvent)@event));
    }

    internal void Configure<TEvent, TValue, TError>(DomainEventHandler<TEvent, TValue, TError> handler)
        where TEvent : IBaseDomainEvent
        where TError : Error
    {
        ArgumentNullException.ThrowIfNull(handler);

        _handlers.Add(Key.From<TEvent, TValue>(), @event => handler((TEvent)@event));
    }

    internal UnitResult<Error> Route<TEvent>(TEvent @event)
        where TEvent : IBaseDomainEvent
    {
        ArgumentNullException.ThrowIfNull(@event);

        var key = Key.From(@event);
        if (!_handlers.TryGetValue(key, out var handler))
        {
            throw new NotSupportedException($"Not found handler for event: `{@event}`");
        }

        return handler(@event);
    }

    internal Result<TValue, Error> Route<TEvent, TValue>(TEvent @event)
        where TEvent : IBaseDomainEvent
    {
        ArgumentNullException.ThrowIfNull(@event);

        var key = Key.From<TValue>(@event);
        if (!_handlers.TryGetValue(key, out var handler))
        {
            throw new NotSupportedException($"Not found handler for event: `{@event}`");
        }

        return handler(@event).MapTry(x => (TValue)x, Error.Because);
    }

    private readonly record struct Key(Type EventType, Maybe<Type> ValueType)
    {
        public static Key From<TEvent>() => new(typeof(TEvent), Maybe.None);

        public static Key From<TEvent, TValue>() => new(typeof(TEvent), typeof(TValue));

        public static Key From(IBaseDomainEvent @event) => new(@event.GetType(), Maybe.None);

        public static Key From<TValue>(IBaseDomainEvent @event) => new(@event.GetType(), typeof(TValue));
    }
}
