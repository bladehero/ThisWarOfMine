using System.IO.Compression;
using ThisWarOfMine.Domain.Abstraction;
using ThisWarOfMine.Domain.Narrative.Events;

namespace ThisWarOfMine.Infrastructure.Books.EventHandlers;

internal sealed class TranslationAlternativeAddedToBookEventHandler
    : IDomainEventHandler<TranslationAlternativeAddedToBookEvent>
{
    private readonly IBookAccessor _bookAccessor;

    public TranslationAlternativeAddedToBookEventHandler(IBookAccessor bookAccessor)
    {
        _bookAccessor = bookAccessor;
    }

    public Task Handle(TranslationAlternativeAddedToBookEvent notification, CancellationToken cancellationToken)
    {
        var (bookId, _, storyNumber, language, alternativeId, text) = notification;
        return _bookAccessor.UseAsync(
            bookId,
            async archive =>
            {
                var entry = archive.CreateEntry(
                    $"{storyNumber}/{language.ShortName}/{alternativeId}/content",
                    CompressionLevel.Optimal
                );
                await using var stream = entry.Open();
                await using var writer = new StreamWriter(stream);
                await writer.WriteAsync(text);
            }
        );
    }
}
