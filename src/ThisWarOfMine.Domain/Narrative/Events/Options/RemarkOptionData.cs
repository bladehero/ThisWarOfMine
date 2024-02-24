namespace ThisWarOfMine.Domain.Narrative.Events.Options
{
    public sealed record RemarkOptionData(Guid Id, int Order, string Remark) : IOptionData;
}
