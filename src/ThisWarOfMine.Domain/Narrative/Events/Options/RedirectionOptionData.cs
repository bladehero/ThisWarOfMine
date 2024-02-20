namespace ThisWarOfMine.Domain.Narrative.Events.Options;

public sealed record RedirectionOptionData(
    Guid Id,
    int Order,
    int StoryNumber,
    string? Text = null,
    string? Appendix = null
) : IOptionData;
