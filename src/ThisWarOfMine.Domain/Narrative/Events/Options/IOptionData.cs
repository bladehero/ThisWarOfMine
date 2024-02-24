namespace ThisWarOfMine.Domain.Narrative.Events.Options
{
    public interface IOptionData
    {
        Guid Id { get; }
        int Order { get; }
    }
}
