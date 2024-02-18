using CSharpFunctionalExtensions;
using ThisWarOfMine.Domain.Narrative;

namespace ThisWarOfMine.Splitter;

public interface IStoryParser
{
    Result<Story> ParseIn(Book book, Language language, IReadOnlyCollection<string> rows);
}