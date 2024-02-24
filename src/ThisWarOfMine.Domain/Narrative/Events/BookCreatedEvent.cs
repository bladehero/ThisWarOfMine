namespace ThisWarOfMine.Domain.Narrative.Events
{
    public sealed record BookCreatedEvent(Guid BookId, DateTime Timestamp, string Name)
        : BaseBookEvent(BookId, Timestamp);
}
