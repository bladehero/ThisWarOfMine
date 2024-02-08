using ThisWarOfMine.Contracts;
using ThisWarOfMine.Contracts.Narrative;

namespace ThisWarOfMine.Splitter;

public interface IStoryCreator
{
    Story Create(Language language, IReadOnlyCollection<string> rows);
}