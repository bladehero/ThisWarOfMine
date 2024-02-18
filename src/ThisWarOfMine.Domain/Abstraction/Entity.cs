using CSharpFunctionalExtensions;

namespace ThisWarOfMine.Domain.Abstraction;

public abstract class Entity<T> : CSharpFunctionalExtensions.Entity<T>
    where T : IComparable<T>
{
    protected InstanceEventRouter Router { get; } = new();

    public bool HasSame(T id) => Id.CompareTo(id) == 0;

    protected void Register<TEvent>(DomainEventHandler<TEvent> handler)
        where TEvent : IBaseDomainEvent =>
        RegisterWithError<TEvent, Error>(x =>
        {
            handler(x);
            return UnitResult.Success<Error>();
        });

    protected void Register<TEvent>(DomainEventHandler<TEvent, Error> handler)
        where TEvent : IBaseDomainEvent => RegisterWithError(handler);

    protected void Register<TEvent, TValue>(DomainEventHandler<TEvent, TValue, Error> handler)
        where TEvent : IBaseDomainEvent
    {
        ArgumentNullException.ThrowIfNull(handler);

        Router.Configure(handler);
    }

    protected void RegisterWithError<TEvent, TError>(DomainEventHandler<TEvent, TError> handler)
        where TEvent : IBaseDomainEvent
        where TError : Error
    {
        ArgumentNullException.ThrowIfNull(handler);

        Router.Configure(handler);
    }

    protected void RegisterWithError<TEvent, TValue, TError>(DomainEventHandler<TEvent, TValue, TError> handler)
        where TEvent : IBaseDomainEvent
        where TError : Error
    {
        ArgumentNullException.ThrowIfNull(handler);

        Router.Configure(handler);
    }

    internal UnitResult<Error> Route<TEvent>(TEvent @event)
        where TEvent : IBaseDomainEvent
    {
        ArgumentNullException.ThrowIfNull(@event);

        return Router.Route(@event);
    }

    internal Result<TValue, Error> Route<TEvent, TValue>(TEvent @event)
        where TEvent : IBaseDomainEvent
    {
        ArgumentNullException.ThrowIfNull(@event);

        return Router.Route<TEvent, TValue>(@event);
    }
}
