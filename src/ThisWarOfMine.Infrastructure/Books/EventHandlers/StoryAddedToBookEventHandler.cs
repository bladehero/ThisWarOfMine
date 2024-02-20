using ThisWarOfMine.Domain.Abstraction;
using ThisWarOfMine.Domain.Narrative.Events;

namespace ThisWarOfMine.Infrastructure.Books.EventHandlers;

internal sealed class StoryAddedToBookEventHandler : IDomainEventHandler<StoryAddedToBookEvent>
{
    private readonly IBookAccessor _bookAccessor;

    public StoryAddedToBookEventHandler(IBookAccessor bookAccessor)
    {
        _bookAccessor = bookAccessor;
    }

    public Task Handle(StoryAddedToBookEvent notification, CancellationToken cancellationToken)
    {
        var (bookId, _, storyNumber) = notification;

        return _bookAccessor.UseAsync(
            bookId,
            archive =>
                Task.Factory.StartNew(
                    () =>
                    {
                        archive.CreateEntry($"{storyNumber.Value}/");
                    },
                    cancellationToken
                )
        );
    }
}
