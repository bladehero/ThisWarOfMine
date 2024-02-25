using CSharpFunctionalExtensions;
using ThisWarOfMine.Domain.Narrative.Events.Options;

namespace ThisWarOfMine.Splitter.Options;

internal interface IOptionParser
{
    Result<IOptionData> Parse(string optionRow, int order);
}
