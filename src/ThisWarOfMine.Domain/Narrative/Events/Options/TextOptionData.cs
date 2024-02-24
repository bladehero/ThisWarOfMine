namespace ThisWarOfMine.Domain.Narrative.Events.Options
{
    public sealed record TextOptionData(Guid Id, int Order, string Text, bool WithBackToGame) : IOptionData;
}
