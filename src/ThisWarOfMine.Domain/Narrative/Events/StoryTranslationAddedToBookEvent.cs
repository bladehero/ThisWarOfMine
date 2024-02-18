namespace ThisWarOfMine.Domain.Narrative.Events;

internal sealed record StoryTranslationAddedToBookEvent(Guid BookId, StoryNumber Number, Language Language)
    : BaseBookEvent(BookId);
