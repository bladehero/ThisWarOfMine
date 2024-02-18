using ThisWarOfMine.Domain.Narrative;

namespace ThisWarOfMine.Splitter;

internal interface IBookCreator
{
    ValueTask<Book> CreateAsync(string name, string path, Language language, CancellationToken token = default);
}