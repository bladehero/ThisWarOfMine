namespace ThisWarOfMine.Domain.Narrative.Events;

public sealed record StoryTranslationAddedToBookEvent(
    Guid BookId,
    DateTime Timestamp,
    StoryNumber Number,
    Language Language
) : BaseBookEvent(BookId, Timestamp);
