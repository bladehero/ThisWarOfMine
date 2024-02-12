using ThisWarOfMine.Contracts.Narrative.Options;

namespace ThisWarOfMine.Splitter.Options;

internal sealed class OptionParser : IOptionParser
{
    private readonly IEnumerable<IOptionParsingStrategy> _strategies;

    public OptionParser(IEnumerable<IOptionParsingStrategy> strategies)
    {
        _strategies = strategies;
    }

    public void ParseIn(OptionGroup group, string optionRow)
    {
        foreach (var strategy in _strategies)
        {
            if (strategy.TryParseIn(group, optionRow))
            {
                return;
            }
        }

        throw new InvalidOperationException($"Cannot find suitable parsing strategy for option: `{optionRow}`");
    }
}