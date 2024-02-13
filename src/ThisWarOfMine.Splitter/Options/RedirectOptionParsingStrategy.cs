using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using ThisWarOfMine.Domain.Narrative.Options;

namespace ThisWarOfMine.Splitter.Options;

internal sealed partial class RedirectOptionParsingStrategy : IOptionParsingStrategy
{
    private const string RedirectionNumberMandatoryError = "Redirection should always have group of number in pattern";
    private const string Prepending = nameof(Prepending);
    private const string Appending = nameof(Appending);
    private const string Number = nameof(Number);

    private static readonly Regex RedirectionRule = GetRedirectionRegex();

    public bool TryParseIn(OptionGroup optionGroup, string optionRow)
    {
        var match = RedirectionRule.Match(optionRow);
        if (!match.Success)
        {
            return false;
        }

        var number = match.Groups.TryFind(Number).Map(AsInteger).GetValueOrThrow(RedirectionNumberMandatoryError);
        var prepending = match.Groups.TryFind(Prepending).Map(AsString).GetValueOrDefault();
        var appending = match.Groups.TryFind(Appending).Map(AsString).GetValueOrDefault();

        optionGroup.AppendWithRedirection(number, prepending, appending);
        return true;

        int AsInteger(Capture group) => int.Parse(group.Value);

        string? AsString(Capture group) => group.Value.Trim();
    }

    [GeneratedRegex($"^\\s*\\?\\s*(?<{Prepending}>.*)[Сс][Мм]\\.*\\s*(?<{Number}>\\d+)\\.?(?<{Appending}>.*)")]
    private static partial Regex GetRedirectionRegex();
}