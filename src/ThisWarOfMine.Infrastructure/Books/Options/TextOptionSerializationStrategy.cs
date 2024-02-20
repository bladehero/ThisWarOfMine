using ThisWarOfMine.Domain.Narrative.Events.Options;

namespace ThisWarOfMine.Infrastructure.Books.Options;

internal sealed class TextOptionSerializationStrategy : BaseOptionSerializationStrategy<TextOptionData>
{
    protected override string Serialize(TextOptionData optionData)
    {
        var (_, _, text, withBackToGame) = optionData;
        return $"{text} | {withBackToGame}";
    }

    protected override TextOptionData Deserialize(Guid guid, int order, string @string)
    {
        var index = @string.LastIndexOf('|');
        if (index <= 0)
        {
            throw new InvalidOperationException($"Cannot parse text option from: `{@string}`");
        }

        var text = @string[..(index - 1)];
        var flag = @string[(index + 1)..];
        return new TextOptionData(guid, order, text, bool.Parse(flag));
    }
}
