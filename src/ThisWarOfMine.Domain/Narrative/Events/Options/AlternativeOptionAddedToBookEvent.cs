namespace ThisWarOfMine.Domain.Narrative.Events.Options;

public sealed record AlternativeOptionAddedToBookEvent(
    Guid BookId,
    DateTime Timestamp,
    StoryNumber Number,
    Language Language,
    Guid AlternativeId,
    IOptionData OptionData
) : BaseBookEvent(BookId, Timestamp);
