using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using ThisWarOfMine.Common.Wrappers;
using ThisWarOfMine.Domain.Narrative.Events.Options;

namespace ThisWarOfMine.Splitter.Options;

internal sealed partial class BackToStoryOptionParsingStrategy : IOptionParsingStrategy
{
    private const string Number = nameof(Number);
    private const string BackMarker = "НАЗАД";
    private const string ToMarker = "К";
    private static readonly Regex BackToStoryRule = GetBackToStoryRegex();

    private readonly IGuidProvider _guidProvider;

    public BackToStoryOptionParsingStrategy(IGuidProvider guidProvider)
    {
        _guidProvider = guidProvider;
    }

    public Maybe<IOptionData> TryParse(string optionRow, int order)
    {
        var match = BackToStoryRule.Match(optionRow);
        if (!match.Success)
        {
            return Maybe.None;
        }

        var number = short.Parse(match.Groups[Number].Value);
        return new RedirectionOptionData(_guidProvider.NewGuid(), order, number, "Вернитесь назад");
    }

    [GeneratedRegex($"^\\s*\\?\\s*({BackMarker})\\s+({ToMarker})\\s+(?<{Number}>\\d+)\\s*\\.?\\s*$")]
    private static partial Regex GetBackToStoryRegex();
}
