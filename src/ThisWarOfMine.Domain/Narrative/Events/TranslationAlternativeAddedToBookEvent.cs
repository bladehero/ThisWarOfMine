namespace ThisWarOfMine.Domain.Narrative.Events;

internal sealed record TranslationAlternativeAddedToBookEvent(
    Guid BookId,
    StoryNumber Number,
    Language Language,
    Guid AlternativeId,
    string Text
) : BaseBookEvent(BookId);
