using System.IO.Compression;
using ThisWarOfMine.Domain.Abstraction;
using ThisWarOfMine.Domain.Narrative.Events.Options;
using ThisWarOfMine.Infrastructure.Books.Options;

namespace ThisWarOfMine.Infrastructure.Books.EventHandlers;

internal sealed class AlternativeOptionAddedToBookEventHandler : IDomainEventHandler<AlternativeOptionAddedToBookEvent>
{
    private readonly IBookAccessor _bookAccessor;
    private readonly IOptionDataSerializer _optionDataSerializer;

    public AlternativeOptionAddedToBookEventHandler(
        IBookAccessor bookAccessor,
        IOptionDataSerializer optionDataSerializer
    )
    {
        _bookAccessor = bookAccessor;
        _optionDataSerializer = optionDataSerializer;
    }

    public Task Handle(AlternativeOptionAddedToBookEvent notification, CancellationToken cancellationToken)
    {
        var (bookId, _, storyNumber, language, alternativeId, optionData) = notification;
        return _bookAccessor.UseAsync(
            bookId,
            async archive =>
            {
                var entry = archive.CreateEntry(
                    $"{storyNumber}/{language.ShortName}/{alternativeId}/options/{optionData.Id}",
                    CompressionLevel.Optimal
                );
                entry.Comment = optionData.Order.ToString();
                await using var stream = entry.Open();
                await using var writer = new StreamWriter(stream);
                var option = _optionDataSerializer.Serialize(optionData);
                await writer.WriteAsync(option);
            }
        );
    }
}
