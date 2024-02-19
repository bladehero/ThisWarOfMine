namespace ThisWarOfMine.Domain.Narrative.Events;

internal sealed record StoryTranslationAddedToBookEvent(
    Guid BookId,
    DateTime Timestamp,
    StoryNumber Number,
    Language Language
) : BaseBookEvent(BookId, Timestamp);
