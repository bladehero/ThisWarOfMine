namespace ThisWarOfMine.Domain.Narrative.Events.Options;

public sealed record RedirectionOptionData(int StoryNumber, string? Text = null, string? Appendix = null) : IOptionData;
