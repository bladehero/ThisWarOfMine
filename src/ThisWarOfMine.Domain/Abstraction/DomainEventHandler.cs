using CSharpFunctionalExtensions;

namespace ThisWarOfMine.Domain.Abstraction
{
    public delegate void DomainEventHandler<in TEvent>(TEvent @event)
        where TEvent : IBaseDomainEvent;

    public delegate UnitResult<TError> DomainEventHandler<in TEvent, TError>(TEvent @event)
        where TEvent : IBaseDomainEvent
        where TError : Error;

    public delegate Result<TValue, TError> DomainEventHandler<in TEvent, TValue, TError>(TEvent @event)
        where TEvent : IBaseDomainEvent
        where TError : Error;
}
