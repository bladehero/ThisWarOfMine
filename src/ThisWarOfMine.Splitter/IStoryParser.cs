using ThisWarOfMine.Domain;
using ThisWarOfMine.Domain.Narrative;

namespace ThisWarOfMine.Splitter;

public interface IStoryParser
{
    void ParseIn(Book book, Language language, IReadOnlyCollection<string> rows);
}