using ThisWarOfMine.Domain.Narrative.Events.Options;

namespace ThisWarOfMine.Infrastructure.Books.Options;

internal sealed class BackToGameOptionSerializationStrategy : BaseOptionSerializationStrategy<BackToGameOptionData>
{
    protected override string Serialize(BackToGameOptionData optionData) => string.Empty;

    protected override BackToGameOptionData Deserialize(Guid guid, int order, string @string) => new(guid, order);
}
