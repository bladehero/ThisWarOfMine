using System.Text.RegularExpressions;
using ThisWarOfMine.Contracts.Narrative.Options;

namespace ThisWarOfMine.Splitter.Options;

internal sealed partial class RemarkOptionParsingStrategy : IOptionParsingStrategy
{
    private static readonly Regex StartFromQuestionMarkRule = GetStartFromQuestionMarkRegex();

    public bool TryParseIn(OptionGroup optionGroup, string optionRow)
    {
        if (StartFromQuestionMarkRule.IsMatch(optionRow))
        {
            return false;
        }

        optionGroup.Note(optionRow);
        return true;
    }

    [GeneratedRegex("^\\s*\\?\\s*")]
    private static partial Regex GetStartFromQuestionMarkRegex();
}