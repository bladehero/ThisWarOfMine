using System.Text.RegularExpressions;
using ThisWarOfMine.Contracts.Narrative.Options;

namespace ThisWarOfMine.Splitter.Options;

internal sealed partial class OnlyBackToGameOptionParsingStrategy : IOptionParsingStrategy
{
    private static readonly Regex OnlyBackToGameRule = GetOnlyBackToGameRegex();
    
    public bool TryParseIn(OptionGroup optionGroup, string optionRow)
    {
        if (!OnlyBackToGameRule.IsMatch(optionRow))
        {
            return false;
        }

        optionGroup.WithOnlyBackToGame();
        return true;

    }
    
    
    
    
    [GeneratedRegex($"^\\s*\\?\\s*({Constants.BackToGameMarker})?\\s*\\.?\\s*$")]
    private static partial Regex GetOnlyBackToGameRegex();
}