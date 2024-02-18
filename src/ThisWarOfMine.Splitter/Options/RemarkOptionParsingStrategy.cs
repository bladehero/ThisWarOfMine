using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using ThisWarOfMine.Domain.Narrative.Events.Options;

namespace ThisWarOfMine.Splitter.Options;

internal sealed partial class RemarkOptionParsingStrategy : IOptionParsingStrategy
{
    private static readonly Regex StartFromQuestionMarkRule = GetStartFromQuestionMarkRegex();

    public Maybe<IOptionData> TryParse(string optionRow)
    {
        if (StartFromQuestionMarkRule.IsMatch(optionRow))
        {
            return Maybe.None;
        }

        return new RemarkOptionData(optionRow);
    }

    [GeneratedRegex("^\\s*\\?\\s*")]
    private static partial Regex GetStartFromQuestionMarkRegex();
}
