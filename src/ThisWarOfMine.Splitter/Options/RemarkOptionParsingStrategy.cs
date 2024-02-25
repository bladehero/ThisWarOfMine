using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using ThisWarOfMine.Common.Wrappers;
using ThisWarOfMine.Domain.Narrative.Events.Options;

namespace ThisWarOfMine.Splitter.Options;

internal sealed partial class RemarkOptionParsingStrategy : IOptionParsingStrategy
{
    private static readonly Regex StartFromQuestionMarkRule = GetStartFromQuestionMarkRegex();
    private readonly IGuidProvider _guidProvider;

    public RemarkOptionParsingStrategy(IGuidProvider guidProvider)
    {
        _guidProvider = guidProvider;
    }

    public Maybe<IOptionData> TryParse(string optionRow, int order)
    {
        if (StartFromQuestionMarkRule.IsMatch(optionRow))
        {
            return Maybe.None;
        }

        return new RemarkOptionData(_guidProvider.NewGuid(), order, optionRow);
    }

    [GeneratedRegex("^\\s*\\?\\s*")]
    private static partial Regex GetStartFromQuestionMarkRegex();
}
