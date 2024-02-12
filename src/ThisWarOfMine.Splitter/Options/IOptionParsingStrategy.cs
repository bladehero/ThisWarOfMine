using ThisWarOfMine.Contracts.Narrative.Options;

namespace ThisWarOfMine.Splitter.Options;

internal interface IOptionParsingStrategy
{
    bool TryParseIn(OptionGroup optionGroup, string optionRow);
}