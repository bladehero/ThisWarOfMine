using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using ThisWarOfMine.Domain.Narrative.Events.Options;

namespace ThisWarOfMine.Splitter.Options;

internal sealed partial class TextOptionParsingStrategy : IOptionParsingStrategy
{
    private const string Text = nameof(Text);
    private const string BackToGame = nameof(BackToGame);
    private static readonly Regex SimpleStoryWithBackToGameRule = GetSimpleStoryWithBackToGameRegex();

    public Maybe<IOptionData> TryParse(string optionRow)
    {
        var match = SimpleStoryWithBackToGameRule.Match(optionRow);
        if (!match.Success)
        {
            return Maybe.None;
        }

        var text = match.Groups[Text].Value;
        var withBackToGame = !string.IsNullOrWhiteSpace(match.Groups[BackToGame].Value);
        return new TextOptionData(text, withBackToGame);
    }


    [GeneratedRegex($"^\\s*\\?\\s*(?<{Text}>.+?)(?<{BackToGame}>{Constants.BackToGameMarker})?\\s*\\.?\\s*$")]
    private static partial Regex GetSimpleStoryWithBackToGameRegex();
}