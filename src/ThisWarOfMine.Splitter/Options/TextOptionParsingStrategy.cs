using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using ThisWarOfMine.Common.Wrappers;
using ThisWarOfMine.Domain.Narrative.Events.Options;

namespace ThisWarOfMine.Splitter.Options;

internal sealed partial class TextOptionParsingStrategy : IOptionParsingStrategy
{
    private const string Text = nameof(Text);
    private const string BackToGame = nameof(BackToGame);
    private static readonly Regex SimpleStoryWithBackToGameRule = GetSimpleStoryWithBackToGameRegex();

    private readonly IGuidProvider _guidProvider;

    public TextOptionParsingStrategy(IGuidProvider guidProvider)
    {
        _guidProvider = guidProvider;
    }

    public Maybe<IOptionData> TryParse(string optionRow, int order)
    {
        var match = SimpleStoryWithBackToGameRule.Match(optionRow);
        if (!match.Success)
        {
            return Maybe.None;
        }

        var text = match.Groups[Text].Value;
        var withBackToGame = !string.IsNullOrWhiteSpace(match.Groups[BackToGame].Value);
        return new TextOptionData(_guidProvider.NewGuid(), order, text, withBackToGame);
    }

    [GeneratedRegex($"^\\s*\\?\\s*(?<{Text}>.+?)(?<{BackToGame}>{Constants.BackToGameMarker})?\\s*\\.?\\s*$")]
    private static partial Regex GetSimpleStoryWithBackToGameRegex();
}
