using MediatR;

namespace ThisWarOfMine.Domain.Abstraction
{
    public interface IBaseDomainEvent : INotification
    {
        DateTime Timestamp { get; }
    }
}
