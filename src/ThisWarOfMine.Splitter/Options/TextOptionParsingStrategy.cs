using System.Text.RegularExpressions;
using ThisWarOfMine.Domain.Narrative.Options;

namespace ThisWarOfMine.Splitter.Options;

internal sealed partial class TextOptionParsingStrategy : IOptionParsingStrategy
{
    private const string Text = nameof(Text);
    private const string BackToGame = nameof(BackToGame);
    private static readonly Regex SimpleStoryWithBackToGameRule = GetSimpleStoryWithBackToGameRegex();
    
    public bool TryParseIn(OptionGroup optionGroup, string optionRow)
    {
        var match = SimpleStoryWithBackToGameRule.Match(optionRow);
        if (!match.Success)
        {
            return false;
        }

        var text = match.Groups[Text].Value;
        var withBackToGame = !string.IsNullOrWhiteSpace(match.Groups[BackToGame].Value);
        optionGroup.AppendWithText(text, withBackToGame);
        return true;
    }
    
    
    [GeneratedRegex($"^\\s*\\?\\s*(?<{Text}>.+?)(?<{BackToGame}>{Constants.BackToGameMarker})?\\s*\\.?\\s*$")]
    private static partial Regex GetSimpleStoryWithBackToGameRegex();
}