using ThisWarOfMine.Contracts.Narrative.Options;

namespace ThisWarOfMine.Splitter.Options;

internal interface IOptionParser
{
    void ParseIn(OptionGroup group, string optionRow);
}