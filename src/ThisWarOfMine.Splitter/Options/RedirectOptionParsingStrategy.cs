using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using ThisWarOfMine.Common.Wrappers;
using ThisWarOfMine.Domain.Narrative.Events.Options;

namespace ThisWarOfMine.Splitter.Options;

internal sealed partial class RedirectOptionParsingStrategy : IOptionParsingStrategy
{
    private const string RedirectionNumberMandatoryError = "Redirection should always have group of number in pattern";
    private const string Prepending = nameof(Prepending);
    private const string Appending = nameof(Appending);
    private const string Number = nameof(Number);
    private static readonly Regex RedirectionRule = GetRedirectionRegex();

    private readonly IGuidProvider _guidProvider;

    public RedirectOptionParsingStrategy(IGuidProvider guidProvider)
    {
        _guidProvider = guidProvider;
    }

    public Maybe<IOptionData> TryParse(string optionRow, int order)
    {
        var match = RedirectionRule.Match(optionRow);
        if (!match.Success)
        {
            return Maybe.None;
        }

        var number = match.Groups.TryFind(Number).Map(AsInteger).GetValueOrThrow(RedirectionNumberMandatoryError);
        var prepending = match.Groups.TryFind(Prepending).Map(AsString).GetValueOrDefault();
        var appending = match.Groups.TryFind(Appending).Map(AsString).GetValueOrDefault();
        return new RedirectionOptionData(_guidProvider.NewGuid(), order, number, prepending, appending);

        int AsInteger(Capture group) => int.Parse(group.Value);

        string? AsString(Capture group) => group.Value.Trim();
    }

    [GeneratedRegex($"^\\s*\\?\\s*(?<{Prepending}>.*)[Сс][Мм]\\.*\\s*(?<{Number}>\\d+)\\.?(?<{Appending}>.*)")]
    private static partial Regex GetRedirectionRegex();
}
