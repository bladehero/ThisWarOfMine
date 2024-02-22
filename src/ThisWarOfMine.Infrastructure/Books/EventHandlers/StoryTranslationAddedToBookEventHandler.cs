using ThisWarOfMine.Domain.Abstraction;
using ThisWarOfMine.Domain.Narrative.Events;

namespace ThisWarOfMine.Infrastructure.Books.EventHandlers;

internal sealed class StoryTranslationAddedToBookEventHandler : IDomainEventHandler<StoryTranslationAddedToBookEvent>
{
    private readonly IBookAccessor _bookAccessor;

    public StoryTranslationAddedToBookEventHandler(IBookAccessor bookAccessor)
    {
        _bookAccessor = bookAccessor;
    }

    public Task Handle(StoryTranslationAddedToBookEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
