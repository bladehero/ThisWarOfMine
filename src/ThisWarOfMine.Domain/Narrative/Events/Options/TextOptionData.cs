namespace ThisWarOfMine.Domain.Narrative.Events.Options;

public sealed record TextOptionData(string Text, bool WithBackToGame) : IOptionData;
