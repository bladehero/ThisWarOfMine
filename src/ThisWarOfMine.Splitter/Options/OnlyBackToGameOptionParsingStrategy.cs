using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using ThisWarOfMine.Domain.Narrative.Events.Options;

namespace ThisWarOfMine.Splitter.Options;

internal sealed partial class OnlyBackToGameOptionParsingStrategy : IOptionParsingStrategy
{
    private static readonly Regex OnlyBackToGameRule = GetOnlyBackToGameRegex();

    public Maybe<IOptionData> TryParse(string optionRow)
    {
        if (!OnlyBackToGameRule.IsMatch(optionRow))
        {
            return Maybe.None;
        }

        return new BackToGameOptionData();
    }

    [GeneratedRegex($"^\\s*\\?\\s*({Constants.BackToGameMarker})\\s*\\.?\\s*$")]
    private static partial Regex GetOnlyBackToGameRegex();
}