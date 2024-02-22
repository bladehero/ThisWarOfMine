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
        return Task.CompletedTask;
    }
}
