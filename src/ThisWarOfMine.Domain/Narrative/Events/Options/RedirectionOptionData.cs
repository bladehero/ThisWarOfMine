namespace ThisWarOfMine.Domain.Narrative.Events.Options;

public sealed record RedirectionOptionData(
    Guid Id,
    int Order,
    short StoryNumber,
    string? Text = null,
    string? Appendix = null
) : IOptionData;
