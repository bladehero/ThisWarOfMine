using ThisWarOfMine.Domain.Abstraction;

namespace ThisWarOfMine.Domain.Narrative.Events;

public abstract record BaseBookEvent(Guid BookId, DateTime Timestamp) : IBaseDomainEvent;
