using CSharpFunctionalExtensions;
using ThisWarOfMine.Domain.Narrative.Events.Options;

namespace ThisWarOfMine.Splitter.Options;

internal interface IOptionParsingStrategy
{
    Maybe<IOptionData> TryParse(string optionRow);
}
