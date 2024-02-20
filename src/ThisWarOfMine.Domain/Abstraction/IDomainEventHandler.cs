using MediatR;

namespace ThisWarOfMine.Domain.Abstraction;

public interface IDomainEventHandler<in T> : INotificationHandler<T>
    where T : IBaseDomainEvent { }
