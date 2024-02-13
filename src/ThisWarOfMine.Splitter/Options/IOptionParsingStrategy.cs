using ThisWarOfMine.Domain.Narrative.Options;

namespace ThisWarOfMine.Splitter.Options;

internal interface IOptionParsingStrategy
{
    bool TryParseIn(OptionGroup optionGroup, string optionRow);
}