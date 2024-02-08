using ThisWarOfMine.Contracts;
using ThisWarOfMine.Contracts.Narrative;

namespace ThisWarOfMine.Splitter;

internal interface IStoryCreator
{
    Story Create(Language language, IReadOnlyCollection<string> rows);
}