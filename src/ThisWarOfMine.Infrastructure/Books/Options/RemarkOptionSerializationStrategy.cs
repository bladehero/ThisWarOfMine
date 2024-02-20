using ThisWarOfMine.Domain.Narrative.Events.Options;

namespace ThisWarOfMine.Infrastructure.Books.Options;

internal sealed class RemarkOptionSerializationStrategy : BaseOptionSerializationStrategy<RemarkOptionData>
{
    protected override string Serialize(RemarkOptionData optionData) => optionData.Remark;

    protected override RemarkOptionData Deserialize(Guid guid, int order, string @string) => new(guid, order, @string);
}
