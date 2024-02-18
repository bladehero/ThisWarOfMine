namespace ThisWarOfMine.Domain.Narrative.Events;

internal sealed record TranslationAlternativeAddedToBookEvent
    (Guid BookId, StoryNumber Number, Language Language, string Text) : BaseBookEvent(BookId);