using ThisWarOfMine.Contracts;
using ThisWarOfMine.Contracts.Narrative;

namespace ThisWarOfMine.Splitter;

public interface IStoryParser
{
    void ParseIn(Book book, Language language, IReadOnlyCollection<string> rows);
}