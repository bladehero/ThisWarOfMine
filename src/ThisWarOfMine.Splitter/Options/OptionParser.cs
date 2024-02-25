using CSharpFunctionalExtensions;
using ThisWarOfMine.Domain.Narrative.Events.Options;

namespace ThisWarOfMine.Splitter.Options;

internal sealed class OptionParser : IOptionParser
{
    private readonly IEnumerable<IOptionParsingStrategy> _strategies;

    public OptionParser(IEnumerable<IOptionParsingStrategy> strategies)
    {
        _strategies = strategies;
    }

    public Result<IOptionData> Parse(string optionRow, int order)
    {
        foreach (var strategy in _strategies)
        {
            var option = strategy.TryParse(optionRow, order);
            if (option.HasValue)
            {
                return Result.Success(option.Value);
            }
        }

        return Result.Failure<IOptionData>($"Cannot find suitable parsing strategy for option: `{optionRow}`");
    }
}
