namespace ThisWarOfMine.Domain.Narrative.Events;

public sealed record StoryAddedToBookEvent(Guid BookId, DateTime Timestamp, StoryNumber Number)
    : BaseBookEvent(BookId, Timestamp);
