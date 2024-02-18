using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using ThisWarOfMine.Domain.Narrative.Events.Options;

namespace ThisWarOfMine.Splitter.Options;

internal sealed partial class BackToStoryOptionParsingStrategy : IOptionParsingStrategy
{
    private const string Number = nameof(Number);
    private const string BackMarker = "НАЗАД";
    private const string ToMarker = "К";
    private static readonly Regex BackToStoryRule = GetBackToStoryRegex();

    public Maybe<IOptionData> TryParse(string optionRow)
    {
        var match = BackToStoryRule.Match(optionRow);
        if (!match.Success)
        {
            return Maybe.None;
        }

        var number = int.Parse(match.Groups[Number].Value);
        return new RedirectionOptionData(number);
    }

    [GeneratedRegex($"^\\s*\\?\\s*({BackMarker})\\s+({ToMarker})\\s+(?<{Number}>\\d+)\\s*\\.?\\s*$")]
    private static partial Regex GetBackToStoryRegex();
}