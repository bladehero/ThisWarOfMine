namespace ThisWarOfMine.Domain.Narrative.Events.Options;

public sealed record BackToGameOptionData(Guid Id, int Order) : IOptionData;
