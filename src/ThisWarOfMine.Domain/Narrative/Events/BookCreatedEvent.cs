namespace ThisWarOfMine.Domain.Narrative.Events;

public sealed record BookCreatedEvent(Guid BookId, string Name) : BaseBookEvent(BookId);
