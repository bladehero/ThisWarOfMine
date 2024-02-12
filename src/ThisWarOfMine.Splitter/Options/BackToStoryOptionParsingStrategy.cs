using System.Text.RegularExpressions;
using ThisWarOfMine.Contracts.Narrative.Options;

namespace ThisWarOfMine.Splitter.Options;

internal sealed partial class BackToStoryOptionParsingStrategy : IOptionParsingStrategy
{
    private const string Number = nameof(Number);
    private const string BackMarker = "НАЗАД";
    private const string ToMarker = "К";
    private static readonly Regex BackToStoryRule = GetBackToStoryRegex();

    public bool TryParseIn(OptionGroup optionGroup, string optionRow)
    {
        var match = BackToStoryRule.Match(optionRow);
        if (!match.Success)
        {
            return false;
        }

        var number = int.Parse(match.Groups[Number].Value);
        optionGroup.AppendWithRedirection(number);
        return true;
    }

    [GeneratedRegex($"^\\s*\\?\\s*({BackMarker})\\s+({ToMarker})\\s+(?<{Number}>\\d+)\\s*\\.?\\s*$")]
    private static partial Regex GetBackToStoryRegex();
}