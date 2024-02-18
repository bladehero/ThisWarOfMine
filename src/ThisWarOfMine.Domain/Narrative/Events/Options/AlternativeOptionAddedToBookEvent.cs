namespace ThisWarOfMine.Domain.Narrative.Events.Options;

internal sealed record AlternativeOptionAddedToBookEvent(
    Guid BookId,
    StoryNumber Number,
    Language Language,
    Guid AlternativeId,
    IOptionData OptionData
) : BaseBookEvent(BookId);