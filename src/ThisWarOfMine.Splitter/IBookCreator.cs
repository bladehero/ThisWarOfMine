using ThisWarOfMine.Domain.Narrative;

namespace ThisWarOfMine.Splitter;

internal interface IBookCreator
{
    Task<Book> InitializeAsync(string name, CancellationToken token = default);
    IAsyncEnumerable<Story> FulFillAsync(string path, Language language, CancellationToken token = default);
}
