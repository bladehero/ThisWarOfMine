namespace ThisWarOfMine.Domain.Narrative.Events;

public sealed record TranslationAlternativeAddedToBookEvent(
    Guid BookId,
    DateTime Timestamp,
    StoryNumber Number,
    Language Language,
    Guid AlternativeId,
    string Text
) : BaseBookEvent(BookId, Timestamp);
