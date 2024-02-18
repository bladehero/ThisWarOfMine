namespace ThisWarOfMine.Domain.Narrative.Events;

public sealed record StoryAddedToBookEvent(Guid BookId, StoryNumber Number) : BaseBookEvent(BookId);