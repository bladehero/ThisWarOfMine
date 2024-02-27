using System.Text;
using System.Text.RegularExpressions;
using ThisWarOfMine.Domain.Narrative.Events.Options;

namespace ThisWarOfMine.Infrastructure.Books.Options;

internal sealed partial class RedirectionOptionSerializationStrategy
    : BaseOptionSerializationStrategy<RedirectionOptionData>
{
    private const string Text = nameof(Text);
    private const string StoryNumber = nameof(StoryNumber);
    private const string Appendix = nameof(Appendix);
    private const string TextPrefix = "-- ";
    private const string StoryNumberPrefix = "-> ";
    private const string AppendixPrefix = "... ";
    private static readonly Regex Regex = GetRegex();

    protected override string Serialize(RedirectionOptionData optionData)
    {
        var builder = new StringBuilder();
        if (!string.IsNullOrWhiteSpace(optionData.Text))
        {
            builder.Append(TextPrefix).Append(optionData.Text);
        }

        builder.Append(StoryNumberPrefix).Append(optionData.StoryNumber);

        if (!string.IsNullOrWhiteSpace(optionData.Appendix))
        {
            builder.Append(AppendixPrefix).AppendLine(optionData.Appendix);
        }

        return builder.ToString();
    }

    protected override RedirectionOptionData Deserialize(Guid guid, int order, string @string)
    {
        var match = Regex.Match(@string);
        if (!match.Success)
        {
            throw new InvalidOperationException($"Cannot deserialize `{@string}` as redirection option data");
        }

        var text = match.Groups[Text].Value;
        var storyNumber = match.Groups[StoryNumber].Value;
        var appendix = match.Groups[Appendix].Value;

        return new RedirectionOptionData(guid, order, short.Parse(storyNumber), text, appendix);
    }

    [GeneratedRegex($"(-- (?<{Text}>.*))?(\\s*-> (?<{StoryNumber}>\\d*\\s*))(... (?<{Appendix}>.*))?\\s*")]
    private static partial Regex GetRegex();
}
