using System.Diagnostics.CodeAnalysis;
using ThisWarOfMine.Domain.Abstraction;

namespace ThisWarOfMine.Domain.Narrative.Events;

[SuppressMessage("ReSharper", "NotAccessedPositionalProperty.Global")]
public abstract record BaseBookEvent(Guid BookId, DateTime Timestamp) : IBaseDomainEvent;
